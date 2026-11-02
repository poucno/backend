using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Poucno.Backend.Api.Data;
using Poucno.Backend.Api.Data.Entities;
using Poucno.Backend.Api.Endpoints;
using Poucno.Backend.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

string[] disabledAuthPaths =
[
    "/auth/confirmEmail",
    "/auth/resendConfirmationEmail",
    "/auth/forgotPassword",
    "/auth/resetPassword",
    "/auth/manage"
];

// Add services to the container.
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Poucno Backend API";
        document.Info.Version = "1.0.0";
        document.Info.Description = "REST API for the Poucno application.";
        return Task.CompletedTask;
    });
    
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "Identity token",
            Description = "Paste the accessToken from POST /auth/login (without the word Bearer)."
        };

        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            }
        ];
        return Task.CompletedTask;
    });
    
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        foreach (var path in document.Paths.Keys
                     .Where(p => disabledAuthPaths.Any(d => p.StartsWith(d, StringComparison.OrdinalIgnoreCase)))
                     .ToList())
        {
            document.Paths.Remove(path);
        }
        return Task.CompletedTask;
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("data/keys"));

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/openapi/v1.json",
        "Poucno API v1"
    );
});

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

var auth = app.MapGroup("/auth").WithTags("Auth");

auth.AddEndpointFilter(async (ctx, next) =>
{
    var path = ctx.HttpContext.Request.Path.Value ?? "";
    return disabledAuthPaths.Any(d => path.StartsWith(d, StringComparison.OrdinalIgnoreCase))
        ? Results.NotFound()
        : await next(ctx);
});

auth.MapIdentityApi<AppUser>();

// Endpoints
app.MapGameEndpoints();
app.MapCollectionEndpoints();
app.MapTodoEndpoints();

await app.RunAsync();

namespace Poucno.Backend.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        services.AddAutoMapper(cfg =>
            cfg.AddMaps(assembly));

        return services;
    }
}

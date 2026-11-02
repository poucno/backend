using Poucno.Backend.Api.Features.Games;

namespace Poucno.Backend.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var assembly = typeof(Program).Assembly;

        services.Configure<GamesOptions>(configuration.GetSection(GamesOptions.SectionName));

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        services.AddAutoMapper(cfg =>
            cfg.AddMaps(assembly));

        return services;
    }
}

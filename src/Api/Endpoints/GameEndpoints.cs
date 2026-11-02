using MediatR;
using Poucno.Backend.Api.Features.Games.Queries;

namespace Poucno.Backend.Api.Endpoints;

public static class GameEndpoints
{
    public static IEndpointRouteBuilder MapGameEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games")
            .WithTags("Games");

        group.MapGet("/", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var games = await sender.Send(
                    new GetGamesQuery(), ct);

                return Results.Ok(games);
            })
            .WithName("GetGames");

        return app;
    }
}

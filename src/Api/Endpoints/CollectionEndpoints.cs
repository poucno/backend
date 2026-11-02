using System.Security.Claims;
using MediatR;
using Microsoft.Extensions.Options;
using Poucno.Backend.Api.Features.Collections;
using Poucno.Backend.Api.Features.Collections.Commands;
using Poucno.Backend.Api.Features.Collections.Dtos;
using Poucno.Backend.Api.Features.Collections.Queries;
using Poucno.Backend.Api.Features.Games;

namespace Poucno.Backend.Api.Endpoints;

public static class CollectionEndpoints
{
    public static IEndpointRouteBuilder MapCollectionEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/collections")
            .WithTags("Collections")
            .RequireAuthorization();

        group.MapGet("/", async (
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var collections = await sender.Send(
                    new GetCollectionsQuery(OwnerId(user)), ct);

                return Results.Ok(collections);
            })
            .WithName("GetCollections");

        group.MapGet("/{id:guid}", async (
                Guid id,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var collection = await sender.Send(
                    new GetCollectionByIdQuery(OwnerId(user), id), ct);

                return collection is null
                    ? Results.NotFound()
                    : Results.Ok(collection);
            })
            .WithName("GetCollectionById");

        group.MapPost("/", async (
                CollectionInput input,
                ClaimsPrincipal user,
                IOptions<GamesOptions> games,
                ISender sender,
                CancellationToken ct) =>
            {
                var errors = CollectionValidator.Validate(input, games.Value.Items);
                if (errors.Count > 0)
                    return Results.ValidationProblem(errors);

                var id = await sender.Send(
                    new CreateCollectionCommand(OwnerId(user), input), ct);

                return Results.Created(
                    $"/collections/{id}",
                    new { id });
            })
            .WithName("CreateCollection");

        group.MapPut("/{id:guid}", async (
                Guid id,
                CollectionInput input,
                ClaimsPrincipal user,
                IOptions<GamesOptions> games,
                ISender sender,
                CancellationToken ct) =>
            {
                var errors = CollectionValidator.Validate(input, games.Value.Items);
                if (errors.Count > 0)
                    return Results.ValidationProblem(errors);

                var updated = await sender.Send(
                    new UpdateCollectionCommand(OwnerId(user), id, input), ct);

                return updated
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("UpdateCollection");

        group.MapDelete("/{id:guid}", async (
                Guid id,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken ct) =>
            {
                var deleted = await sender.Send(
                    new DeleteCollectionCommand(OwnerId(user), id), ct);

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("DeleteCollection");

        return app;
    }

    private static string OwnerId(ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier)!;
}

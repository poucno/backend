using MediatR;
using Poucno.Backend.Api.Features.Todos.Commands;
using Poucno.Backend.Api.Features.Todos.Queries;

namespace Poucno.Backend.Api.Endpoints;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/todos")
            .WithTags("Todos");

        group.MapGet("/", async (
                ISender sender,
                CancellationToken ct) =>
            {
                var todos = await sender.Send(
                    new GetTodosQuery(), ct);

                return Results.Ok(todos);
            })
            .WithName("GetTodos");

        group.MapGet("/{id:guid}", async (
                Guid id,
                ISender sender,
                CancellationToken ct) =>
            {
                var todo = await sender.Send(
                    new GetTodoByIdQuery(id), ct);

                return todo is null
                    ? Results.NotFound()
                    : Results.Ok(todo);
            })
            .WithName("GetTodoById");

        group.MapPost("/", async (
                CreateTodoCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                if (string.IsNullOrWhiteSpace(command.Title))
                    return Results.BadRequest("Title is required.");

                var id = await sender.Send(command, ct);

                return Results.Created(
                    $"/api/todos/{id}",
                    new { id });
            })
            .WithName("CreateTodo");

        group.MapPatch("/{id:guid}/complete", async (
                Guid id,
                ISender sender,
                CancellationToken ct) =>
            {
                var completed = await sender.Send(
                    new CompleteTodoCommand(id), ct);

                return completed
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("CompleteTodo");
        
        group.MapDelete("/{id:guid}", async (
                Guid id,
                ISender sender,
                CancellationToken ct) =>
            {
                var deleted = await sender.Send(
                    new DeleteTodoCommand(id), ct);

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("DeleteTodo");

        return app;
    }
}

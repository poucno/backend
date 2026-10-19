using MediatR;
using Poucno.Backend.Api.Data;
using Poucno.Backend.Api.Data.Entities;

namespace Poucno.Backend.Api.Features.Todos.Commands;

public record CreateTodoCommand(string Title) : IRequest<Guid>;

public class CreateTodoCommandHandler(AppDbContext db) : IRequestHandler<CreateTodoCommand, Guid>
{
    public async Task<Guid> Handle(CreateTodoCommand request, CancellationToken ct)
    {
        var todo = new TodoItem
        {
            Title = request.Title.Trim()
        };

        db.TodoItems.Add(todo);

        await db.SaveChangesAsync(ct);

        return todo.Id;
    }
}

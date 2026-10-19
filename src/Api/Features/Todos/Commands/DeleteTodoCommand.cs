using MediatR;
using Microsoft.EntityFrameworkCore;
using Poucno.Backend.Api.Data;

namespace Poucno.Backend.Api.Features.Todos.Commands;

public record DeleteTodoCommand(Guid Id) : IRequest<bool>;

public class DeleteTodoCommandHandler(AppDbContext db) : IRequestHandler<DeleteTodoCommand, bool>
{
    public async Task<bool> Handle(
        DeleteTodoCommand request,
        CancellationToken ct)
    {
        var todo = await db.TodoItems
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (todo is null)
            return false;

        db.TodoItems.Remove(todo);

        await db.SaveChangesAsync(ct);

        return true;
    }
}

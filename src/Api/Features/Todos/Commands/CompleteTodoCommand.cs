using MediatR;
using Microsoft.EntityFrameworkCore;
using Poucno.Backend.Api.Data;

namespace Poucno.Backend.Api.Features.Todos.Commands;

public record CompleteTodoCommand(Guid Id) : IRequest<bool>;

public class CompleteTodoCommandHandler(AppDbContext db) : IRequestHandler<CompleteTodoCommand, bool>
{
    public async Task<bool> Handle(
        CompleteTodoCommand request,
        CancellationToken ct)
    {
        var todo = await db.TodoItems
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (todo is null)
            return false;

        todo.IsCompleted = true;

        await db.SaveChangesAsync(ct);

        return true;
    }
}

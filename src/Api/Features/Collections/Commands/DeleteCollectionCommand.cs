using MediatR;
using Microsoft.EntityFrameworkCore;
using Poucno.Backend.Api.Data;

namespace Poucno.Backend.Api.Features.Collections.Commands;

public record DeleteCollectionCommand(string OwnerId, Guid Id) : IRequest<bool>;

public class DeleteCollectionCommandHandler(AppDbContext db) : IRequestHandler<DeleteCollectionCommand, bool>
{
    public async Task<bool> Handle(DeleteCollectionCommand request, CancellationToken ct)
    {
        var collection = await db.Collections
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.OwnerId == request.OwnerId, ct);

        if (collection is null)
            return false;

        db.Collections.Remove(collection);

        await db.SaveChangesAsync(ct);

        return true;
    }
}

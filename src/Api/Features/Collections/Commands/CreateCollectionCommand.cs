using MediatR;
using Poucno.Backend.Api.Data;
using Poucno.Backend.Api.Data.Entities;
using Poucno.Backend.Api.Features.Collections.Dtos;
using Poucno.Backend.Api.Features.Collections.Mappings;

namespace Poucno.Backend.Api.Features.Collections.Commands;

public record CreateCollectionCommand(string OwnerId, CollectionInput Input) : IRequest<Guid>;

public class CreateCollectionCommandHandler(AppDbContext db) : IRequestHandler<CreateCollectionCommand, Guid>
{
    public async Task<Guid> Handle(CreateCollectionCommand request, CancellationToken ct)
    {
        var input = request.Input;

        var collection = new Collection
        {
            OwnerId = request.OwnerId,
            Name = input.Name.Trim(),
            Subject = input.Subject.Trim(),
            Grade = input.Grade,
            Description = input.Description?.Trim() ?? "",
            GameId = input.GameId,
            Questions = CollectionQuestionFactory.Create(input.Questions)
        };

        db.Collections.Add(collection);

        await db.SaveChangesAsync(ct);

        return collection.Id;
    }
}

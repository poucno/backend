using MediatR;
using Microsoft.EntityFrameworkCore;
using Poucno.Backend.Api.Data;
using Poucno.Backend.Api.Features.Collections.Dtos;
using Poucno.Backend.Api.Features.Collections.Mappings;

namespace Poucno.Backend.Api.Features.Collections.Commands;

public record UpdateCollectionCommand(string OwnerId, Guid Id, CollectionInput Input) : IRequest<bool>;

public class UpdateCollectionCommandHandler(AppDbContext db) : IRequestHandler<UpdateCollectionCommand, bool>
{
    public async Task<bool> Handle(UpdateCollectionCommand request, CancellationToken ct)
    {
        var collection = await db.Collections
            .Include(c => c.Questions)
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.OwnerId == request.OwnerId, ct);

        if (collection is null)
            return false;

        var input = request.Input;

        collection.Name = input.Name.Trim();
        collection.Subject = input.Subject.Trim();
        collection.Grade = input.Grade;
        collection.Description = input.Description?.Trim() ?? "";
        collection.GameId = input.GameId;
        collection.UpdatedAt = DateTime.UtcNow;

        // The questions are replaced as a whole, like the frontend editor saves them.
        db.CollectionQuestions.RemoveRange(collection.Questions);

        var questions = CollectionQuestionFactory.Create(input.Questions);
        foreach (var question in questions)
            question.CollectionId = collection.Id;

        // AddRange marks them as new; attaching them through the navigation would treat their preset ids as existing rows.
        db.CollectionQuestions.AddRange(questions);

        await db.SaveChangesAsync(ct);

        return true;
    }
}

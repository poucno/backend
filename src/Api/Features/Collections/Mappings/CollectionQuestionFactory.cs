using Poucno.Backend.Api.Data.Entities;
using Poucno.Backend.Api.Features.Collections.Dtos;

namespace Poucno.Backend.Api.Features.Collections.Mappings;

public static class CollectionQuestionFactory
{
    public static List<CollectionQuestion> Create(IEnumerable<QuestionDto>? questions)
    {
        return (questions ?? [])
            .Select((q, index) => new CollectionQuestion
            {
                Order = index,
                Text = q.Question.Trim(),
                Hint = string.IsNullOrWhiteSpace(q.Hint) ? null : q.Hint.Trim(),
                PossibleAnswers = (q.PossibleAnswers ?? []).Select(a => a.Trim()).ToList(),
                CorrectAnswer = q.Answer.Trim()
            })
            .ToList();
    }
}

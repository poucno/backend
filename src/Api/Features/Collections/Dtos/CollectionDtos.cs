namespace Poucno.Backend.Api.Features.Collections.Dtos;

public record QuestionDto(
    string Question,
    string? Hint,
    List<string> PossibleAnswers,
    string Answer
);

public record CollectionDto(
    Guid Id,
    string Name,
    string Subject,
    int Grade,
    string Description,
    string GameId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<QuestionDto> Questions
);

public record CollectionSummaryDto(
    Guid Id,
    string Name,
    string Subject,
    int Grade,
    string GameId,
    int QuestionCount,
    DateTime UpdatedAt
);

public record CollectionInput(
    string Name,
    string Subject,
    int Grade,
    string? Description,
    string GameId,
    List<QuestionDto>? Questions
);

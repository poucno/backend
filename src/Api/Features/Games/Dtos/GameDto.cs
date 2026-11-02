namespace Poucno.Backend.Api.Features.Games.Dtos;

public record GameDto(
    string Id,
    string Title,
    string Url,
    string ThumbnailUrl,
    int MinAnswers,
    int MaxAnswers
);

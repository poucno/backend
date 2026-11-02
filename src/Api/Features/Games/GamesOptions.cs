namespace Poucno.Backend.Api.Features.Games;

public sealed class GamesOptions
{
    public const string SectionName = "Games";

    public List<GameOption> Items { get; set; } = [];
}

public sealed class GameOption
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string ThumbnailUrl { get; set; } = null!;
}

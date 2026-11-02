namespace Poucno.Backend.Api.Data.Entities;

public sealed class CollectionQuestion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CollectionId { get; set; }
    public int Order { get; set; }
    public string Text { get; set; } = null!;
    public string? Hint { get; set; }
    public List<string> PossibleAnswers { get; set; } = [];
    public string CorrectAnswer { get; set; } = null!;
}

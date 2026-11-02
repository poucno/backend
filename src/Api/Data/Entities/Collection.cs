namespace Poucno.Backend.Api.Data.Entities;

public sealed class Collection
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string OwnerId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public int Grade { get; set; }
    public string Description { get; set; } = "";
    public string GameId { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<CollectionQuestion> Questions { get; set; } = [];
}

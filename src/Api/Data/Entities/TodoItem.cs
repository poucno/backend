namespace Poucno.Backend.Api.Data.Entities;

public sealed class TodoItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; }
}

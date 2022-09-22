namespace Core.Model;

public sealed class NoteModel
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Content { get; init; } = null!;
}
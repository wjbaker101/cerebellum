namespace Core.Model;

public sealed class NoteModel
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
}
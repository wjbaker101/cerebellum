namespace Core.Model;

public sealed class NoteModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
}
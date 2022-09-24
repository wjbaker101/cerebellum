using Core.Model;

namespace Api.Notes.Types;

public sealed class UpdateNoteRequest
{
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
}

public sealed class UpdateNoteResponse
{
    public NoteModel Note { get; init; } = null!;
}
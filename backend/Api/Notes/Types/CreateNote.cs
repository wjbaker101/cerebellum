using Core.Model;

namespace Api.Notes.Types;

public sealed class CreateNoteRequest
{
    public string Content { get; init; } = null!;
}

public sealed class CreateNoteResponse
{
    public NoteModel Note { get; init; } = null!;
}
using Core.Model;

namespace Api.Api.Notes.Types;

public sealed class CreateNoteRequest
{
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
}

public sealed class CreateNoteResponse
{
    public NoteModel Note { get; init; } = null!;
}
using Core.Model;

namespace Api.Api.Notes.Types;

public sealed class CreateNoteRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }
}

public sealed class CreateNoteResponse
{
    public required NoteModel Note { get; init; }
}
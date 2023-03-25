using Core.Model;

namespace Api.Api.Notes.Types;

public sealed class UpdateNoteRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }
}

public sealed class UpdateNoteResponse
{
    public required NoteModel Note { get; init; }
}
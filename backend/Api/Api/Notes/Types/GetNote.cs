using Core.Model;

namespace Api.Api.Notes.Types;

public sealed class GetNoteResponse
{
    public required NoteModel Note { get; init; }
}
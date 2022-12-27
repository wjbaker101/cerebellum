using Core.Model;

namespace Api.Api.Notes.Types;

public sealed class GetNoteResponse
{
    public NoteModel Note { get; init; } = null!;
}
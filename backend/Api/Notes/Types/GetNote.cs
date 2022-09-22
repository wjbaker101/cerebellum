using Core.Model;

namespace Api.Notes.Types;

public sealed class GetNoteResponse
{
    public NoteModel Note { get; init; } = null!;
}
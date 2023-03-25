using Core.Model;

namespace Cerebellum.Api.Notes.Types;

public sealed class GetNoteResponse
{
    public required NoteModel Note { get; init; }
}
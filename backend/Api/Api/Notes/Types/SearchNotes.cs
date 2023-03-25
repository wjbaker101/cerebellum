using Core.Model;

namespace Api.Api.Notes.Types;

public sealed class SearchNotesResponse
{
    public required List<NoteModel> Notes { get; init; }
}
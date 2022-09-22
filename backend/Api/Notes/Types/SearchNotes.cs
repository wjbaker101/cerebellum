using Core.Model;

namespace Api.Notes.Types;

public sealed class SearchNotesResponse
{
    public List<NoteModel> Notes { get; init; } = new();
}
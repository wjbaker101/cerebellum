using Core.Model;

namespace Api.Api.Notes.Types;

public sealed class SearchNotesResponse
{
    public List<NoteModel> Notes { get; init; } = new();
}
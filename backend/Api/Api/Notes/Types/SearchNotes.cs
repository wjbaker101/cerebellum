using Core.Model;

namespace Cerebellum.Api.Notes.Types;

public sealed class SearchNotesResponse
{
    public required List<NoteModel> Notes { get; init; }
}
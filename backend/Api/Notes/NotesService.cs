using Api.Notes.Types;
using Core.Model;
using Data.Repositories;
using NetApiLibs.Type;

namespace Api.Notes;

public interface INotesService
{
    Result<SearchNotesResponse> SearchNotes();
}

public sealed class NotesService : INotesService
{
    private readonly INotesRepository _notesRepository;

    public NotesService(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }

    public Result<SearchNotesResponse> SearchNotes()
    {
        var notes = _notesRepository.SearchNotes();

        return new SearchNotesResponse
        {
            Notes = notes.ConvertAll(x => new NoteModel
            {
                Reference = x.Reference,
                CreatedAt = x.CreatedAt,
                Content = x.Content
            })
        };
    }
}
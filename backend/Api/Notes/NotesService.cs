using Api.Notes.Types;
using Core.Model;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Type;

namespace Api.Notes;

public interface INotesService
{
    Result<SearchNotesResponse> SearchNotes();
    Result<CreateNoteResponse> CreateNote(CreateNoteRequest request);
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

    public Result<CreateNoteResponse> CreateNote(CreateNoteRequest request)
    {
        var note = _notesRepository.SaveNote(new NoteRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Content = request.Content
        });

        return new CreateNoteResponse
        {
            Note = new NoteModel
            {
                Reference = note.Reference,
                CreatedAt = note.CreatedAt,
                Content = note.Content
            }
        };
    }
}
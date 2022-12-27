using Api.Api.Notes.Types;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Type;

namespace Api.Api.Notes;

public interface INotesService
{
    Result<SearchNotesResponse> SearchNotes();
    Result<CreateNoteResponse> CreateNote(CreateNoteRequest request);
    Result<GetNoteResponse> GetNote(Guid reference);
    Result<UpdateNoteResponse> UpdateNote(Guid reference, UpdateNoteRequest request);
    Result DeleteNote(Guid reference);
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
            Notes = notes.ConvertAll(NoteMapper.Map)
        };
    }

    public Result<CreateNoteResponse> CreateNote(CreateNoteRequest request)
    {
        var note = _notesRepository.SaveNote(new NoteRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Title = request.Title,
            Content = request.Content
        });

        return new CreateNoteResponse
        {
            Note = NoteMapper.Map(note)
        };
    }
    
    public Result<GetNoteResponse> GetNote(Guid reference)
    {
        var noteResult = _notesRepository.GetByReference(reference);
        if (noteResult.IsFailure)
            return Result<GetNoteResponse>.FromFailure(noteResult);

        return new GetNoteResponse
        {
            Note = NoteMapper.Map(noteResult.Value)
        };
    }

    public Result<UpdateNoteResponse> UpdateNote(Guid reference, UpdateNoteRequest request)
    {
        var noteResult = _notesRepository.GetByReference(reference);
        if (!noteResult.TrySuccess(out var note))
            return Result<UpdateNoteResponse>.FromFailure(noteResult);

        note.Title = request.Title;
        note.Content = request.Content;

        _notesRepository.UpdateNote(note);

        return new UpdateNoteResponse
        {
            Note = NoteMapper.Map(note)
        };
    }

    public Result DeleteNote(Guid reference)
    {
        var noteResult = _notesRepository.GetByReference(reference);
        if (noteResult.IsFailure)
            return Result<GetNoteResponse>.FromFailure(noteResult);

        _notesRepository.DeleteNote(noteResult.Value);

        return Result.Success();
    }
}
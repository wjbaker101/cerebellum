using Data.Records;
using NetApiLibs.Type;
using System.Net;

namespace Data.Repositories;

public interface INotesRepository
{
    NoteRecord SaveNote(NoteRecord entry);
    List<NoteRecord> SearchNotes();
    Result<NoteRecord> GetByReference(Guid reference);
    void DeleteNote(NoteRecord note);
}

public sealed class NotesRepository : BaseRepository, INotesRepository
{
    private readonly IApiDatabase _database;

    public NotesRepository(IApiDatabase database) : base(database)
    {
        _database = database;
    }

    public NoteRecord SaveNote(NoteRecord entry) => SaveRecord(entry);

    public List<NoteRecord> SearchNotes()
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var notes = session
            .Query<NoteRecord>()
            .ToList();

        transaction.Commit();

        return notes;
    }

    public Result<NoteRecord> GetByReference(Guid reference)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var note = session
            .Query<NoteRecord>()
            .SingleOrDefault(x => x.Reference == reference);

        transaction.Commit();

        if (note == null)
            return Result<NoteRecord>.Failure($"Unable to find note with given reference: {reference}.", HttpStatusCode.NotFound);

        return note;
    }

    public void DeleteNote(NoteRecord note)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Delete(note);

        transaction.Commit();
    }
}
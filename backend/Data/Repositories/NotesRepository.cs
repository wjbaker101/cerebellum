using Data.Records;
using DotNetLibs.Core.Types;

namespace Data.Repositories;

public interface INotesRepository
{
    NoteRecord SaveNote(NoteRecord entry);
    NoteRecord UpdateNote(NoteRecord note);
    List<NoteRecord> SearchNotes();
    Result<NoteRecord> GetByReference(Guid reference);
    void DeleteNote(NoteRecord note);
}

public sealed class NotesRepository : BaseRepository, INotesRepository
{
    public NotesRepository(IApiDatabase database) : base(database)
    {
    }

    public NoteRecord SaveNote(NoteRecord note) => SaveRecord(note);
    public NoteRecord UpdateNote(NoteRecord note) => UpdateRecord(note);

    public List<NoteRecord> SearchNotes()
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var notes = session
            .Query<NoteRecord>()
            .ToList();

        transaction.Commit();

        return notes;
    }

    public Result<NoteRecord> GetByReference(Guid reference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var note = session
            .Query<NoteRecord>()
            .SingleOrDefault(x => x.Reference == reference);

        transaction.Commit();

        if (note == null)
            return Result<NoteRecord>.Failure($"Unable to find note with given reference: {reference}.");

        return note;
    }

    public void DeleteNote(NoteRecord note)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Delete(note);

        transaction.Commit();
    }
}
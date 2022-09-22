using Data.Records;
using NetApiLibs.Type;

namespace Data.Repositories;

public interface INotesRepository
{
    Result<NoteRecord> GetByReference(Guid reference);
}

public sealed class NotesRepository : BaseRepository, INotesRepository
{
    private readonly IApiDatabase _database;

    public NotesRepository(IApiDatabase database) : base(database)
    {
        _database = database;
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
            return Result<NoteRecord>.Failure($"Unable to find note with given reference: {reference}.");

        return note;
    }
}
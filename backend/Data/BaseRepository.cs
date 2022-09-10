namespace Data;

public abstract class BaseRepository
{
    private readonly IApiDatabase _database;

    protected BaseRepository(IApiDatabase database)
    {
        _database = database;
    }

    protected T SaveRecord<T>(T record)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Save(record);

        transaction.Commit();

        return record;
    }
}
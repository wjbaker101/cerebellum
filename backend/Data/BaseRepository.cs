namespace Data;

public abstract class BaseRepository
{
    protected readonly IApiDatabase Database;

    protected BaseRepository(IApiDatabase database)
    {
        Database = database;
    }

    protected T SaveRecord<T>(T record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Save(record);

        transaction.Commit();

        return record;
    }

    protected T UpdateRecord<T>(T record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Update(record);

        transaction.Commit();

        return record;
    }

    protected void UpdateManyRecords<T>(IEnumerable<T> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        foreach (var record in records)
            session.Update(record);
         
        transaction.Commit();
    }
}
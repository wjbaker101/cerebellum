using Data.Records;
using NetApiLibs.Type;

namespace Data.Repositories;

public interface ICalendarRepository
{
    Result<CalendarEntryRecord> GetEntryByReference(Guid reference);
}

public sealed class CalendarRepository : ICalendarRepository
{
    private readonly IApiDatabase _database;

    public CalendarRepository(IApiDatabase database)
    {
        _database = database;
    }

    public Result<CalendarEntryRecord> GetEntryByReference(Guid reference)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var entry = session
            .Query<CalendarEntryRecord>()
            .SingleOrDefault();

        transaction.Commit();

        if (entry == null)
            return Result<CalendarEntryRecord>.Failure($"Unable to find calendar entry with given reference: {reference}.");

        return entry;
    }
}
using Data.Records;
using NetApiLibs.Type;

namespace Data.Repositories;

public interface ICalendarRepository
{
    CalendarEntryRecord SaveEntry(CalendarEntryRecord entry);
    Result<CalendarEntryRecord> GetEntryByReference(Guid reference);
}

public sealed class CalendarRepository : BaseRepository, ICalendarRepository
{
    private readonly IApiDatabase _database;

    public CalendarRepository(IApiDatabase database) : base(database)
    {
        _database = database;
    }

    public CalendarEntryRecord SaveEntry(CalendarEntryRecord entry) => SaveRecord(entry);

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
using Data.Records;
using NetApiLibs.Type;

namespace Data.Repositories;

public interface ICalendarRepository
{
    CalendarEntryRecord SaveEntry(CalendarEntryRecord entry);
    Result<CalendarEntryRecord> GetEntryByReference(Guid reference);
    List<CalendarEntryRecord> SearchEntries(DateTime startAt, DateTime endAt);
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

    public List<CalendarEntryRecord> SearchEntries(DateTime startAt, DateTime endAt)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var entries = session
            .Query<CalendarEntryRecord>()
            .Where(x =>
                (x.StartAt >= startAt && x.EndAt <= endAt) ||
                (startAt >= x.StartAt && startAt <= x.EndAt || endAt >= x.StartAt && endAt <= x.EndAt) ||
                (x.RecurringPeriod != CalendarEntryRecurringPeriod.None))
            .ToList();

        transaction.Commit();

        return entries;
    }
}
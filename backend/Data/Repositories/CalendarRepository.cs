using Data.Records;
using DotNetLibs.Core.Types;

namespace Data.Repositories;

public interface ICalendarRepository
{
    CalendarEntryRecord SaveEntry(CalendarEntryRecord entry);
    Result<CalendarEntryRecord> GetEntryByReference(Guid reference);
    List<CalendarEntryRecord> SearchEntries(DateTime startAt, DateTime endAt);
}

public sealed class CalendarRepository : BaseRepository, ICalendarRepository
{
    public CalendarRepository(IApiDatabase database) : base(database)
    {
    }

    public CalendarEntryRecord SaveEntry(CalendarEntryRecord entry) => SaveRecord(entry);

    public Result<CalendarEntryRecord> GetEntryByReference(Guid reference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var entry = session
            .Query<CalendarEntryRecord>()
            .SingleOrDefault(x => x.Reference == reference);

        transaction.Commit();

        if (entry == null)
            return Result<CalendarEntryRecord>.Failure($"Unable to find calendar entry with given reference: {reference}.");

        return entry;
    }

    public List<CalendarEntryRecord> SearchEntries(DateTime startAt, DateTime endAt)
    {
        using var session = Database.SessionFactory.OpenSession();
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
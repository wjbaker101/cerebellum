using Cerebellum.Api.Calendar.Types;
using Core.Model;
using Data.Records;
using Data.Repositories;
using DotNetLibs.Core.Extensions;
using DotNetLibs.Core.Types;
using CalendarEntryRecurringPeriod = Core.Model.CalendarEntryRecurringPeriod;

namespace Cerebellum.Api.Calendar;

public interface ICalendarService
{
    Result<CreateEntryResponse> CreateEntry(CreateEntryRequest request);
    Result<SearchEntriesResponse> SearchEntries(SearchEntriesRequest request);
}

public sealed class CalendarService : ICalendarService
{
    private readonly ICalendarRepository _calendarRepository;

    public CalendarService(ICalendarRepository calendarRepository)
    {
        _calendarRepository = calendarRepository;
    }

    public Result<CreateEntryResponse> CreateEntry(CreateEntryRequest request)
    {
        if (request.RecurringPeriod == CalendarEntryRecurringPeriod.Unknown)
            return Result<CreateEntryResponse>.Failure("The given recurring period is invalid.");

        var entry = _calendarRepository.SaveEntry(new CalendarEntryRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Description = request.Description,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            RecurringPeriod = (Data.Records.CalendarEntryRecurringPeriod)request.RecurringPeriod
        });

        return new CreateEntryResponse
        {
            CalendarEntry = new CalendarEntryModel
            {
                Reference = entry.Reference,
                CreatedAt = entry.CreatedAt,
                Description = entry.Description,
                StartAt = entry.StartAt,
                EndAt = entry.EndAt,
                RecurringPeriod = (CalendarEntryRecurringPeriod)entry.RecurringPeriod
            }
        };
    }

    public Result<SearchEntriesResponse> SearchEntries(SearchEntriesRequest request)
    {
        var entries = _calendarRepository.SearchEntries(request.StartAt, request.EndAt);

        return new SearchEntriesResponse
        {
            Entries = entries
                .Where(x => ShouldShowEntry(x, request.StartAt, request.EndAt))
                .MapAll(x => new CalendarEntryModel
                {
                    Reference = x.Reference,
                    CreatedAt = x.CreatedAt,
                    Description = x.Description,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    RecurringPeriod = (CalendarEntryRecurringPeriod)x.RecurringPeriod
                })
        };
    }

    private static bool ShouldShowEntry(CalendarEntryRecord entry, DateTime startAt, DateTime endAt)
    {
        if (entry.RecurringPeriod == Data.Records.CalendarEntryRecurringPeriod.Weekly)
            return DoesIntersect((int)entry.StartAt.DayOfWeek, (int)entry.EndAt.DayOfWeek, (int)startAt.DayOfWeek, (int)endAt.DayOfWeek);

        if (entry.RecurringPeriod == Data.Records.CalendarEntryRecurringPeriod.Monthly)
            return DoesIntersect(entry.StartAt.Day, entry.EndAt.Day, startAt.Day, endAt.Day);

        if (entry.RecurringPeriod == Data.Records.CalendarEntryRecurringPeriod.Yearly)
            return DoesIntersect(entry.StartAt.Month, entry.EndAt.Month, startAt.Month, endAt.Month) && DoesIntersect(entry.StartAt.Day, entry.EndAt.Day, startAt.Day, endAt.Day);

        return true;
    }

    private static bool DoesIntersect(int first, int last, int lower, int upper)
    {
        return (first >= lower && last <= upper) || (lower >= first && lower <= last || upper >= first && upper <= last);
    }
}
using Api.Calendar.Types;
using Core.Model;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Type;
using CalendarEntryRecurringPeriod = Core.Model.CalendarEntryRecurringPeriod;

namespace Api.Calendar;

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
            Entries = entries.ConvertAll(x => new CalendarEntryModel
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
}
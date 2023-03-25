using Core.Model;

namespace Api.Api.Calendar.Types;

public sealed class CreateEntryRequest
{
    public required string Description { get; set; }
    public required DateTime StartAt { get; set; }
    public required DateTime EndAt { get; set; }
    public required CalendarEntryRecurringPeriod RecurringPeriod { get; set; }
}

public sealed class CreateEntryResponse
{
    public required CalendarEntryModel CalendarEntry { get; init; }
}
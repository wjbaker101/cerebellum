using Core.Model;

namespace Api.Api.Calendar.Types;

public sealed class CreateEntryRequest
{
    public string Description { get; set; } = null!;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public CalendarEntryRecurringPeriod RecurringPeriod { get; set; }
}

public sealed class CreateEntryResponse
{
    public CalendarEntryModel CalendarEntry { get; init; } = null!;
}
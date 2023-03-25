namespace Core.Model;

public sealed class CalendarEntryModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Description { get; init; }
    public required DateTime StartAt { get; init; }
    public required DateTime EndAt { get; init; }
    public required CalendarEntryRecurringPeriod RecurringPeriod { get; init; }
}

public enum CalendarEntryRecurringPeriod
{
    Unknown = 0,
    None = 1,
    Weekly = 2,
    Monthly = 3,
    Yearly = 4
}
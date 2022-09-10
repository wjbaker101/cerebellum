namespace Core.Model;

public sealed class CalendarEntryModel
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Description { get; init; } = null!;
    public DateTime StartAt { get; init; }
    public DateTime EndAt { get; init; }
    public CalendarEntryRecurringPeriod RecurringPeriod { get; init; }
}

public enum CalendarEntryRecurringPeriod
{
    Unknown = 0,
    None = 1,
    Weekly = 2,
    Monthly = 3,
    Yearly = 4
}
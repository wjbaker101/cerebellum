using FluentNHibernate.Mapping;

namespace Data.Records;

public class CalendarEntryRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual string Description { get; set; } = null!;
    public virtual DateTime StartAt { get; set; }
    public virtual DateTime EndAt { get; set; }
    public virtual CalendarEntryRecurringPeriod RecurringPeriod { get; set; }
}

public enum CalendarEntryRecurringPeriod
{
    Unknown = 0,
    None = 1,
    Weekly = 2,
    Monthly = 3,
    Yearly = 4
}

public sealed class CalendarEntryRecordMap : ClassMap<CalendarEntryRecord>
{
    public CalendarEntryRecordMap()
    {
        Schema("cerebellum");
        Table("calendar_entry");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("calendar_entry_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Description, "description");
        Map(x => x.StartAt, "start_at");
        Map(x => x.EndAt, "end_at");
        Map(x => x.RecurringPeriod, "recurring_period").CustomType<CalendarEntryRecurringPeriod>();
    }
}
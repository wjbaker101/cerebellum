using FluentNHibernate.Mapping;

namespace Data.Records;

public class WorkoutEntrySetRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual int Repetitions { get; set; }
    public virtual decimal Weight { get; set; }
}

public sealed class WorkoutEntrySetRecordMap : ClassMap<WorkoutEntrySetRecord>
{
    public WorkoutEntrySetRecordMap()
    {
        Schema("cerebellum");
        Table("workout_entry_set");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("workout_entry_set_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Repetitions, "repetitions");
        Map(x => x.Weight, "weight");
    }
}
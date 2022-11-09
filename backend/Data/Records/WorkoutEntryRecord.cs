using FluentNHibernate.Mapping;

namespace Data.Records;

public class WorkoutEntryRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual DateTime Date { get; set; }
    public virtual DateTime StartTime { get; set; }
    public virtual DateTime? EndTime { get; set; }
    public virtual decimal? Weight { get; set; }
    public virtual IList<WorkoutEntryExerciseRecord> Exercises { get; set; } = new List<WorkoutEntryExerciseRecord>();
}

public sealed class WorkoutEntryRecordMap : ClassMap<WorkoutEntryRecord>
{
    public WorkoutEntryRecordMap()
    {
        Schema("cerebellum");
        Table("workout_entry");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("workout_entry_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Date, "date");
        Map(x => x.StartTime, "start_time");
        Map(x => x.EndTime, "end_time");
        Map(x => x.Weight, "weight");
        HasMany(x => x.Exercises).KeyColumn("workout_entry_id");
    }
}
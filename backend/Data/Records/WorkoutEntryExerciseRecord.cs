using FluentNHibernate.Mapping;

namespace Data.Records;

public class WorkoutEntryExerciseRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Name { get; set; }
    public virtual required ICollection<WorkoutEntrySetRecord> Sets { get; set; }
    public virtual required WorkoutEntryRecord Entry { get; init; }
}

public sealed class WorkoutEntryExerciseRecordMap : ClassMap<WorkoutEntryExerciseRecord>
{
    public WorkoutEntryExerciseRecordMap()
    {
        Schema("cerebellum");
        Table("workout_entry_exercise");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("workout_entry_exercise_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Name, "name");
        HasMany(x => x.Sets).KeyColumn("workout_entry_exercise_id").Inverse().AsSet();
        References(x => x.Entry, "workout_entry_id");
    }
}
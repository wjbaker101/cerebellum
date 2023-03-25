using FluentNHibernate.Mapping;

namespace Data.Records;

public class WorkoutEntrySetRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required int Repetitions { get; set; }
    public virtual required decimal Weight { get; set; }
    public virtual required WorkoutEntryExerciseRecord Exercise { get; init; }
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
        References(x => x.Exercise, "workout_entry_exercise_id");
    }
}
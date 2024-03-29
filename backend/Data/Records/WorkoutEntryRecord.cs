﻿using FluentNHibernate.Mapping;

namespace Data.Records;

public class WorkoutEntryRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required DateTime StartAt { get; set; }
    public virtual required DateTime? EndAt { get; set; }
    public virtual required decimal? Weight { get; set; }
    public virtual required ICollection<WorkoutEntryExerciseRecord> Exercises { get; set; }
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
        Map(x => x.StartAt, "start_at");
        Map(x => x.EndAt, "end_at");
        Map(x => x.Weight, "weight");
        HasMany(x => x.Exercises).KeyColumn("workout_entry_id").Inverse().AsSet();
    }
}
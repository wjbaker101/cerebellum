using Core.Model;
using Data.Records;
using NetApiLibs.Extension;

namespace Data.Mappers;

public static class WorkoutDiaryMapper
{
    public static WorkoutEntry MapEntry(WorkoutEntryRecord entry)
    {
        return new WorkoutEntry
        {
            Reference = entry.Reference,
            CreatedAt = entry.CreatedAt,
            StartAt = entry.StartAt,
            EndAt = entry.EndAt,
            Weight = entry.Weight,
            Exercises = entry.Exercises
                .OrderBy(x => x.CreatedAt)
                .ConvertAll(MapExercise)
        };
    }

    public static WorkoutEntryExercise MapExercise(WorkoutEntryExerciseRecord exercise)
    {
        return new WorkoutEntryExercise
        {
            Reference = exercise.Reference,
            CreatedAt = exercise.CreatedAt,
            Name = exercise.Name,
            Sets = exercise.Sets
                .OrderBy(x => x.CreatedAt)
                .ConvertAll(MapSet)
        };
    }

    public static WorkoutEntrySet MapSet(WorkoutEntrySetRecord set)
    {
        return new WorkoutEntrySet
        {
            Reference = set.Reference,
            CreatedAt = set.CreatedAt,
            Repetitions = set.Repetitions,
            Weight = set.Weight
        };
    }
}
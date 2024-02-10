using Core.Model;
using Data.Records;
using DotNetLibs.Core.Extensions;

namespace Data.Mappers;

public static class WorkoutDiaryMapper
{
    public static WorkoutEntry MapEntry(WorkoutEntryRecord entry) => new()
    {
        Reference = entry.Reference,
        CreatedAt = entry.CreatedAt,
        StartAt = entry.StartAt,
        EndAt = entry.EndAt,
        Weight = entry.Weight,
        Exercises = entry.Exercises.OrderBy(x => x.CreatedAt).MapAll(MapExercise)
    };

    public static WorkoutEntryExercise MapExercise(WorkoutEntryExerciseRecord exercise) => new()
    {
        Reference = exercise.Reference,
        CreatedAt = exercise.CreatedAt,
        Name = exercise.Name,
        Sets = exercise.Sets.OrderBy(x => x.CreatedAt).MapAll(MapSet)
    };

    public static WorkoutEntrySet MapSet(WorkoutEntrySetRecord set) => new()
    {
        Reference = set.Reference,
        CreatedAt = set.CreatedAt,
        Repetitions = set.Repetitions,
        Weight = set.Weight
    };
}
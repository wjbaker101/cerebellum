namespace Core.Model;

public sealed class WorkoutEntry
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime StartAt { get; init; }
    public required DateTime? EndAt { get; init; }
    public required decimal? Weight { get; init; }
    public required List<WorkoutEntryExercise> Exercises { get; init; }
}

public sealed class WorkoutEntryExercise
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Name { get; init; } 
    public required List<WorkoutEntrySet> Sets { get; init; }
}

public sealed class WorkoutEntrySet
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required int Repetitions { get; init; }
    public required decimal Weight { get; init; }
}
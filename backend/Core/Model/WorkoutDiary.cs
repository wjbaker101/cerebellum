namespace Core.Model;

public sealed class WorkoutEntry
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime Date { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public decimal? Weight { get; init; }
    public List<WorkoutEntryExercise> Exercises { get; init; } = new();
}

public sealed class WorkoutEntryExercise
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Name { get; init; } = null!;
    public List<WorkoutEntrySet> Sets { get; init; } = new();
}

public sealed class WorkoutEntrySet
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public int Repetitions { get; init; }
    public decimal Weight { get; init; }
}
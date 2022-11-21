using Core.Model;

namespace Api.WorkoutDiary.Types;

public sealed class UpdateEntryRequest
{
    public DateTime Date { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public decimal? Weight { get; init; }
    public List<Exercise> Exercises { get; init; } = new();

    public sealed class Exercise
    {
        public Guid? Reference { get; init; }
        public string Name { get; init; } = null!;
        public List<Set> Sets { get; init; } = new();
    }

    public sealed class Set
    {
        public Guid? Reference { get; init; }
        public int Repetitions { get; init; }
        public decimal Weight { get; init; }
    }
}

public sealed class UpdateEntryResponse
{
    public WorkoutEntry Entry { get; init; } = null!;
}
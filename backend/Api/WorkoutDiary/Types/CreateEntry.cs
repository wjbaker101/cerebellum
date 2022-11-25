using Core.Model;

namespace Api.WorkoutDiary.Types;

public sealed class CreateEntryRequest
{
    public DateTime StartAt { get; init; }
    public DateTime? EndAt { get; init; }
    public decimal? Weight { get; init; }
    public List<Exercise> Exercises { get; init; } = new();

    public sealed class Exercise
    {
        public string Name { get; init; } = null!;
        public List<Set> Sets { get; init; } = new();
    }

    public sealed class Set
    {
        public int Repetitions { get; init; }
        public decimal Weight { get; init; }
    }
}

public sealed class CreateEntryResponse
{
    public WorkoutEntry Entry { get; init; } = null!;
}
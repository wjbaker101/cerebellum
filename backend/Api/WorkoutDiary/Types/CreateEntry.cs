using Core.Model;

namespace Api.WorkoutDiary.Types;

public sealed class CreateEntryRequest
{
    public DateTime Date { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime? EndTime { get; init; }
    public decimal? Weight { get; init; }
}

public sealed class CreateEntryResponse
{
    public WorkoutEntry Entry { get; init; } = null!;
}
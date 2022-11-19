using Core.Model;

namespace Api.WorkoutDiary.Types;

public sealed class UpdateEntryRequest
{
    public DateOnly Date { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly? EndTime { get; init; }
    public decimal? Weight { get; init; }
}

public sealed class UpdateEntryResponse
{
    public WorkoutEntry Entry { get; init; } = null!;
}
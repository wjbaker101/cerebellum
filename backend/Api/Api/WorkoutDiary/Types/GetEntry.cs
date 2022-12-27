using Core.Model;

namespace Api.Api.WorkoutDiary.Types;

public sealed class GetEntryResponse
{
    public WorkoutEntry Entry { get; init; } = null!;
}
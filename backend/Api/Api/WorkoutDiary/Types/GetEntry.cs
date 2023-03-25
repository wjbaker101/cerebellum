using Core.Model;

namespace Api.Api.WorkoutDiary.Types;

public sealed class GetEntryResponse
{
    public required WorkoutEntry Entry { get; init; }
}
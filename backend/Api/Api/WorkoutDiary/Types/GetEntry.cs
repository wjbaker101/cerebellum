using Core.Model;

namespace Cerebellum.Api.WorkoutDiary.Types;

public sealed class GetEntryResponse
{
    public required WorkoutEntry Entry { get; init; }
}
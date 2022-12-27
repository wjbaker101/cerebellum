using Core.Model;

namespace Api.Api.WorkoutDiary.Types;

public sealed class GetEntriesResponse
{
    public List<WorkoutEntry> Entries { get; init; } = new();
}
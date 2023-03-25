using Core.Model;

namespace Api.Api.WorkoutDiary.Types;

public sealed class GetEntriesResponse
{
    public required List<WorkoutEntry> Entries { get; init; }
}
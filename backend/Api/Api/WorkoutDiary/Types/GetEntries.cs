using Core.Model;

namespace Cerebellum.Api.WorkoutDiary.Types;

public sealed class GetEntriesResponse
{
    public required List<WorkoutEntry> Entries { get; init; }
}
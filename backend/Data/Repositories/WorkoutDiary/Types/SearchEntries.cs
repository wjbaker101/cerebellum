using Data.Records;

namespace Data.Repositories.WorkoutDiary.Types;

public sealed class SearchEntriesParameters
{
    public required DateTime StartAt { get; init; }
    public required DateTime EndAt { get; init; }
    public required int PageSize { get; init; }
    public required int PageNumber { get; init; }
}

public sealed class SearchEntriesDto
{
    public required List<WorkoutEntryRecord> Entries { get; init; }
    public required int EntryCount { get; init; }
    public required int PageSize { get; init; }
    public required int PageNumber { get; init; }
    public required int PageCount { get; init; }
}
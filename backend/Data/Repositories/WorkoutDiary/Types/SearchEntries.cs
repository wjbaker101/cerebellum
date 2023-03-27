namespace Data.Repositories.WorkoutDiary.Types;

public sealed class SearchEntriesParameters
{
    public required DateTime StartAt { get; init; }
    public required DateTime EndAt { get; init; }
}
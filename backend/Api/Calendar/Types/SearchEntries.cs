using Core.Model;

namespace Api.Calendar.Types;

public sealed class SearchEntriesRequest
{
    public DateTime StartAt { get; init; }
    public DateTime EndAt { get; init; }
}

public sealed class SearchEntriesResponse
{
    public List<CalendarEntryModel> Entries { get; init; } = new();
}
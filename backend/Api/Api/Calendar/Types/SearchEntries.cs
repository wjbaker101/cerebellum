using Core.Model;

namespace Cerebellum.Api.Calendar.Types;

public sealed class SearchEntriesRequest
{
    public required DateTime StartAt { get; init; }
    public required DateTime EndAt { get; init; }
}

public sealed class SearchEntriesResponse
{
    public required List<CalendarEntryModel> Entries { get; init; }
}
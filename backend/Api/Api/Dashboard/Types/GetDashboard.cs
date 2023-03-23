namespace Api.Api.Dashboard.Types;

public sealed class GetDashboardResponse
{
    public List<DashboardItem> Items { get; init; } = new();

    public sealed class DashboardItem
    {
        public Guid Reference { get; init; }
        public string Title { get; init; } = default!;
        public ApiDashboardItemType Type { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public enum ApiDashboardItemType
    {
        Unknown = 0,
        Note = 1,
        List = 2,
        KanbanBoard = 3
    }
}
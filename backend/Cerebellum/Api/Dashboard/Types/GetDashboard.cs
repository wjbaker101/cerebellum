namespace Cerebellum.Api.Dashboard.Types;

public sealed class GetDashboardResponse
{
    public required List<DashboardItem> Items { get; init; }

    public sealed class DashboardItem
    {
        public required Guid Reference { get; init; }
        public required string Title { get; init; }
        public required ApiDashboardItemType Type { get; init; }
        public required DateTime CreatedAt { get; init; }
    }

    public enum ApiDashboardItemType
    {
        Unknown = 0,
        Note = 1,
        List = 2,
        KanbanBoard = 3
    }
}
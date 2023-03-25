namespace Data.Repositories.Dashboard.Types;

public sealed class GetDashboardParameters
{
    public required int MaxItems { get; init; }
}

public sealed class GetDashboardDto
{
    public required List<DashboardItem> Items { get; init; }

    public sealed class DashboardItem
    {
        public required Guid Reference { get; init; }
        public required string Title { get; init; }
        public required DashboardItemType Type { get; init; }
        public required DateTime CreatedAt { get; init; }
    }
}

public enum DashboardItemType
{
    Unknown = 0,
    Note = 1,
    List = 2,
    KanbanBoard = 3
}
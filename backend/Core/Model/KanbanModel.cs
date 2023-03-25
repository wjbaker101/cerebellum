namespace Core.Model;

public sealed class KanbanBoardModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Title { get; init; }
    public required List<KanbanColumnModel> Columns { get; init; }
}

public sealed class KanbanColumnModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Title { get; init; }
    public required int Position { get; init; }
    public required List<KanbanItemModel> Items { get; init; }
}

public sealed class KanbanItemModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Content { get; init; }
    public required int Position { get; init; }
}
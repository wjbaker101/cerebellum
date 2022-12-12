namespace Core.Model;

public sealed class KanbanBoardModel
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Title { get; init; } = null!;
    public List<KanbanColumnModel> Columns { get; init; } = new();
}

public sealed class KanbanColumnModel
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Title { get; init; } = null!;
    public int Position { get; init; }
    public List<KanbanItemModel> Items { get; init; } = new();
}

public sealed class KanbanItemModel
{
    public Guid Reference { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Content { get; init; } = null!;
    public int Position { get; init; }
}
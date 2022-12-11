using Core.Model;

namespace Api.Kanban.Types;

public sealed class AddKanbanColumnRequest
{
    public string Title { get; init; } = null!;
}

public sealed class AddKanbanColumnResponse
{
    public KanbanColumnModel KanbanColumn { get; init; } = null!;
}
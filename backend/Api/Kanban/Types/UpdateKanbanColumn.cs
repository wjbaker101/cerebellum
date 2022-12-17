using Core.Model;

namespace Api.Kanban.Types;

public sealed class UpdateKanbanColumnRequest
{
    public string Title { get; init; } = null!;
}

public sealed class UpdateKanbanColumnResponse
{
    public KanbanColumnModel KanbanColumn { get; init; } = null!;
}
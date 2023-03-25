using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class UpdateKanbanColumnRequest
{
    public required string Title { get; init; }
}

public sealed class UpdateKanbanColumnResponse
{
    public required KanbanColumnModel KanbanColumn { get; init; }
}
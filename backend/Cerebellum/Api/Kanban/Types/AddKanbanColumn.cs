using Core.Model;

namespace Cerebellum.Api.Kanban.Types;

public sealed class AddKanbanColumnRequest
{
    public required string Title { get; init; }
}

public sealed class AddKanbanColumnResponse
{
    public required KanbanColumnModel KanbanColumn { get; init; }
}
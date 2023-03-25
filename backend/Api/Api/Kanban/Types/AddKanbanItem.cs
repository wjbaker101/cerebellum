using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class AddKanbanItemRequest
{
    public required string Content { get; init; }
}

public sealed class AddKanbanItemResponse
{
    public required KanbanItemModel KanbanItem { get; init; }
}
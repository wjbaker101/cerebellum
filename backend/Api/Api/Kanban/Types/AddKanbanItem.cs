using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class AddKanbanItemRequest
{
    public string Content { get; init; } = null!;
}

public sealed class AddKanbanItemResponse
{
    public KanbanItemModel KanbanItem { get; init; } = null!;
}
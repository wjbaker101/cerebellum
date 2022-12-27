using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class UpdateKanbanItemRequest
{
    public string Content { get; init; } = null!;
    public Guid ColumnReference { get; init; }
}

public sealed class UpdateKanbanItemResponse
{
    public KanbanItemModel KanbanItem { get; init; } = null!;
}
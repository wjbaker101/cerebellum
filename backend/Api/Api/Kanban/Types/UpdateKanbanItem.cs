using Core.Model;

namespace Cerebellum.Api.Kanban.Types;

public sealed class UpdateKanbanItemRequest
{
    public required string Content { get; init; }
    public required Guid ColumnReference { get; init; }
}

public sealed class UpdateKanbanItemResponse
{
    public required KanbanItemModel KanbanItem { get; init; }
}
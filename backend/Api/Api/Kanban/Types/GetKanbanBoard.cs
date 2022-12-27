using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class GetKanbanBoardResponse
{
    public KanbanBoardModel KanbanBoard { get; init; } = null!;
}
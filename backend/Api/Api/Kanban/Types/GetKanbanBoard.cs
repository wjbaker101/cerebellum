using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class GetKanbanBoardResponse
{
    public required KanbanBoardModel KanbanBoard { get; init; }
}
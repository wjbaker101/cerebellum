using Core.Model;

namespace Cerebellum.Api.Kanban.Types;

public sealed class GetKanbanBoardResponse
{
    public required KanbanBoardModel KanbanBoard { get; init; }
}
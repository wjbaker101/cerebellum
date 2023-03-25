using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class CreateKanbanBoardRequest
{
    public required string Title { get; init; }
}

public sealed class CreateKanbanBoardResponse
{
    public required KanbanBoardModel KanbanBoard { get; init; }
}
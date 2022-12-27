using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class UpdateKanbanBoardRequest
{
    public string Title { get; init; } = null!;
}

public sealed class UpdateKanbanBoardResponse
{
    public KanbanBoardModel KanbanBoard { get; init; } = null!;
}
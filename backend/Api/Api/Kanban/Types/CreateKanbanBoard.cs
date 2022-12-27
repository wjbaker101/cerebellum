using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class CreateKanbanBoardRequest
{
    public string Title { get; init; } = null!;
}

public sealed class CreateKanbanBoardResponse
{
    public KanbanBoardModel KanbanBoard { get; init; } = null!;
}
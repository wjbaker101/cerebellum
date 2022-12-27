using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class GetKanbanBoardsResponse
{
    public List<KanbanBoardModel> KanbanBoards { get; init; } = new();
}
using Core.Model;

namespace Api.Api.Kanban.Types;

public sealed class GetKanbanBoardsResponse
{
    public required List<KanbanBoardModel> KanbanBoards { get; init; }
}
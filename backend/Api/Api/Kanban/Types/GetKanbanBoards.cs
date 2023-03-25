using Core.Model;

namespace Cerebellum.Api.Kanban.Types;

public sealed class GetKanbanBoardsResponse
{
    public required List<KanbanBoardModel> KanbanBoards { get; init; }
}
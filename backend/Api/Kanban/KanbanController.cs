using Api.Kanban.Types;
using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Api.Kanban;

[Route("api/kanban")]
public sealed class KanbanController : ApiController
{
    private readonly IKanbanService _kanbanService;

    public KanbanController(IKanbanService kanbanService)
    {
        _kanbanService = kanbanService;
    }

    [HttpGet]
    [Route("board/{reference:guid}")]
    public IActionResult GetBoard([FromRoute] Guid reference)
    {
        var result = _kanbanService.GetBoard(reference);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("board")]
    public IActionResult GetBoard([FromBody] CreateKanbanBoardRequest request)
    {
        var result = _kanbanService.CreateKanbanBoard(request);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("board/{boardReference:guid}/column")]
    public IActionResult AddKanbanColumn([FromRoute] Guid boardReference, [FromBody] AddKanbanColumnRequest request)
    {
        var result = _kanbanService.AddKanbanColumn(boardReference, request);

        return ToApiResponse(result);
    }
}
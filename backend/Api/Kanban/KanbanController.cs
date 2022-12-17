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
    [Route("search")]
    public IActionResult GetBoards()
    {
        var result = _kanbanService.GetBoards();

        return ToApiResponse(result);
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

    [HttpDelete]
    [Route("board/{boardReference:guid}/column/{columnReference:guid}")]
    public IActionResult DeleteKanbanColumn([FromRoute] Guid boardReference, [FromRoute] Guid columnReference)
    {
        var result = _kanbanService.DeleteKanbanColumn(boardReference, columnReference);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("board/{boardReference:guid}/positions")]
    public IActionResult UpdateBoardPositions([FromRoute] Guid boardReference, [FromBody] UpdateBoardPositionsRequest request)
    {
        var result = _kanbanService.UpdateBoardPositions(boardReference, request);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("board/{boardReference:guid}/column/{columnReference:guid}/item")]
    public IActionResult AddKanbanItem([FromRoute] Guid boardReference, [FromRoute] Guid columnReference, [FromBody] AddKanbanItemRequest request)
    {
        var result = _kanbanService.AddKanbanItem(boardReference, columnReference, request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("board/{boardReference:guid}/column/{columnReference:guid}/item/{itemReference:guid}")]
    public IActionResult UpdateKanbanItem([FromRoute] Guid boardReference, [FromRoute] Guid columnReference, [FromRoute] Guid itemReference, [FromBody] UpdateKanbanItemRequest request)
    {
        var result = _kanbanService.UpdateKanbanItem(boardReference, columnReference, itemReference, request);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("board/{boardReference:guid}/column/{columnReference:guid}/item/{itemReference:guid}")]
    public IActionResult DeleteKanbanItem([FromRoute] Guid boardReference, [FromRoute] Guid columnReference, [FromRoute] Guid itemReference)
    {
        var result = _kanbanService.DeleteKanbanItem(boardReference, columnReference, itemReference);

        return ToApiResponse(result);
    }
}
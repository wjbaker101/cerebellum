﻿using Cerebellum.Api.Kanban.Types;
using Cerebellum.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace Cerebellum.Api.Kanban;

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
    [Authenticate]
    public IActionResult GetBoards()
    {
        var result = _kanbanService.GetBoards();

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("board/{reference:guid}")]
    [Authenticate]
    public IActionResult GetBoard([FromRoute] Guid reference)
    {
        var result = _kanbanService.GetBoard(reference);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("board")]
    [Authenticate]
    public IActionResult CreateBoard([FromBody] CreateKanbanBoardRequest request)
    {
        var result = _kanbanService.CreateKanbanBoard(request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("board/{reference:guid}")]
    [Authenticate]
    public IActionResult UpdateBoard([FromRoute] Guid reference, [FromBody] UpdateKanbanBoardRequest request)
    {
        var result = _kanbanService.UpdateKanbanBoard(reference, request);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("board/{boardReference:guid}/column")]
    [Authenticate]
    public IActionResult AddKanbanColumn([FromRoute] Guid boardReference, [FromBody] AddKanbanColumnRequest request)
    {
        var result = _kanbanService.AddKanbanColumn(boardReference, request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("board/{boardReference:guid}/column/{columnReference:guid}")]
    [Authenticate]
    public IActionResult UpdateKanbanColumn([FromRoute] Guid boardReference, [FromRoute] Guid columnReference, [FromBody] UpdateKanbanColumnRequest request)
    {
        var result = _kanbanService.UpdateKanbanColumn(boardReference, columnReference, request);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("board/{boardReference:guid}/column/{columnReference:guid}")]
    [Authenticate]
    public IActionResult DeleteKanbanColumn([FromRoute] Guid boardReference, [FromRoute] Guid columnReference)
    {
        var result = _kanbanService.DeleteKanbanColumn(boardReference, columnReference);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("board/{boardReference:guid}/positions")]
    [Authenticate]
    public IActionResult UpdateBoardPositions([FromRoute] Guid boardReference, [FromBody] UpdateBoardPositionsRequest request)
    {
        var result = _kanbanService.UpdateBoardPositions(boardReference, request);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("board/{boardReference:guid}/column/{columnReference:guid}/item")]
    [Authenticate]
    public IActionResult AddKanbanItem([FromRoute] Guid boardReference, [FromRoute] Guid columnReference, [FromBody] AddKanbanItemRequest request)
    {
        var result = _kanbanService.AddKanbanItem(boardReference, columnReference, request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("board/{boardReference:guid}/column/{columnReference:guid}/item/{itemReference:guid}")]
    [Authenticate]
    public IActionResult UpdateKanbanItem([FromRoute] Guid boardReference, [FromRoute] Guid columnReference, [FromRoute] Guid itemReference, [FromBody] UpdateKanbanItemRequest request)
    {
        var result = _kanbanService.UpdateKanbanItem(boardReference, columnReference, itemReference, request);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("board/{boardReference:guid}/column/{columnReference:guid}/item/{itemReference:guid}")]
    [Authenticate]
    public IActionResult DeleteKanbanItem([FromRoute] Guid boardReference, [FromRoute] Guid columnReference, [FromRoute] Guid itemReference)
    {
        var result = _kanbanService.DeleteKanbanItem(boardReference, columnReference, itemReference);

        return ToApiResponse(result);
    }
}
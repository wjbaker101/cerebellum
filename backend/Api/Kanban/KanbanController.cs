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
}
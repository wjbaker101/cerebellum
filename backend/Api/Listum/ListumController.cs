using Api.Listum.Types;
using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Api.Listum;

[Route("api/listum")]
public sealed class ListumController : ApiController
{
    private readonly IListumService _listumService;

    public ListumController(IListumService listumService)
    {
        _listumService = listumService;
    }

    [HttpGet]
    [Route("lists")]
    public IActionResult GetLists()
    {
        var result = _listumService.GetLists();

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("list/{reference:guid}")]
    public IActionResult GetListByReference([FromRoute] Guid reference)
    {
        var result = _listumService.GetListByReference(reference);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("list")]
    public IActionResult CreateList([FromBody] CreateListRequest request)
    {
        var result = _listumService.CreateList(request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("list/{reference:guid}")]
    public IActionResult UpdateList([FromRoute] Guid reference, [FromBody] UpdateListRequest request)
    {
        var result = _listumService.UpdateList(reference, request);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("list/{reference:guid}/reorder")]
    public IActionResult ReorderList([FromRoute] Guid reference, [FromBody] ReorderListRequest request)
    {
        var result = _listumService.ReorderList(reference, request);

        return ToApiResponse(result);
    }
}
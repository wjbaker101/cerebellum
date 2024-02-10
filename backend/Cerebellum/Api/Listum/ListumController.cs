using Cerebellum.Api.Listum.Types;
using Cerebellum.Middleware.Authentication;
using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Cerebellum.Api.Listum;

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
    [Authenticate]
    public IActionResult GetLists()
    {
        var result = _listumService.GetLists();

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("list/{reference:guid}")]
    [Authenticate]
    public IActionResult GetListByReference([FromRoute] Guid reference)
    {
        var result = _listumService.GetListByReference(reference);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("list")]
    [Authenticate]
    public IActionResult CreateList([FromBody] CreateListRequest request)
    {
        var result = _listumService.CreateList(request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("list/{reference:guid}")]
    [Authenticate]
    public IActionResult UpdateList([FromRoute] Guid reference, [FromBody] UpdateListRequest request)
    {
        var result = _listumService.UpdateList(reference, request);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("list/{reference:guid}/reorder")]
    [Authenticate]
    public IActionResult ReorderList([FromRoute] Guid reference, [FromBody] ReorderListRequest request)
    {
        var result = _listumService.ReorderList(reference, request);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("list/{reference:guid}/item")]
    [Authenticate]
    public IActionResult CreateListItem([FromRoute] Guid reference, [FromBody] CreateListItemRequest request)
    {
        var result = _listumService.CreateListItem(reference, request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("list/{listReference:guid}/item/{itemReference:guid}")]
    [Authenticate]
    public IActionResult UpdateListItem([FromRoute] Guid listReference, [FromRoute] Guid itemReference, [FromBody] UpdateListItemRequest request)
    {
        var result = _listumService.UpdateListItem(listReference, itemReference, request);

        return ToApiResponse(result);
    }
}
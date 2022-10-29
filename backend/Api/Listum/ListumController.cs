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
}
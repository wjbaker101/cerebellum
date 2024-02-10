using Cerebellum.Api.Calendar.Types;
using Cerebellum.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace Cerebellum.Api.Calendar;

[Route("api/calendar")]
public sealed class CalendarController : ApiController
{
    private readonly ICalendarService _calendarService;

    public CalendarController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    [HttpPost]
    [Route("entry")]
    [Authenticate]
    public IActionResult CreateEntry([FromBody] CreateEntryRequest request)
    {
        var result = _calendarService.CreateEntry(request);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("entries")]
    [Authenticate]
    public IActionResult SearchEntries([FromQuery] SearchEntriesRequest request)
    {
        var result = _calendarService.SearchEntries(request);

        return ToApiResponse(result);
    }
}
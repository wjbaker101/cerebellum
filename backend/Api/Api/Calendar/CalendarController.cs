using Api.Api.Calendar.Types;
using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Api.Api.Calendar;

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
    public IActionResult CreateEntry([FromBody] CreateEntryRequest request)
    {
        var result = _calendarService.CreateEntry(request);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("entries")]
    public IActionResult SearchEntries([FromQuery] SearchEntriesRequest request)
    {
        var result = _calendarService.SearchEntries(request);

        return ToApiResponse(result);
    }
}
using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Api.Calendar;

[Route("api/calendar")]
public sealed class CalendarController : ApiController
{
    private readonly ICalendarService _calendarService;

    public CalendarController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }
}
using Cerebellum.Api.WorkoutDiary.Types;
using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

namespace Cerebellum.Api.WorkoutDiary;

[Route("api/workout-diary")]
public sealed class WorkoutDiaryController : ApiController
{
    private readonly IWorkoutDiaryService _workoutDiaryService;

    public WorkoutDiaryController(IWorkoutDiaryService workoutDiaryService)
    {
        _workoutDiaryService = workoutDiaryService;
    }

    [HttpGet]
    [Route("entries/search")]
    public IActionResult SearchEntries(
        [FromQuery(Name = "page_number")] int pageNumber,
        [FromQuery(Name = "page_size")] int pageSize,
        [FromQuery(Name = "start_at")] DateTime startAt,
        [FromQuery(Name = "end_at")] DateTime endAt)
    {
        var result = _workoutDiaryService.SearchEntries(new SearchEntriesRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            StartAt = startAt,
            EndAt = endAt
        });

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("entry/{reference:guid}")]
    public IActionResult GetEntry([FromRoute] Guid reference)
    {
        var result = _workoutDiaryService.GetEntryByReference(reference);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("entries")]
    public IActionResult GetEntries()
    {
        var result = _workoutDiaryService.GetEntries();

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("entry")]
    public IActionResult CreateEntry([FromBody] CreateEntryRequest request)
    {
        var result = _workoutDiaryService.CreateEntry(request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("entry/{reference:guid}")]
    public IActionResult UpdateEntry([FromRoute] Guid reference, [FromBody] UpdateEntryRequest request)
    {
        var result = _workoutDiaryService.UpdateEntry(reference, request);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("entry/{reference:guid}")]
    public IActionResult DeleteEntry([FromRoute] Guid reference)
    {
        var result = _workoutDiaryService.DeleteEntry(reference);

        return ToApiResponse(result);
    }
}
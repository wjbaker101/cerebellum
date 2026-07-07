using Core.Types;
using Microsoft.AspNetCore.Mvc;

namespace Cerebellum.Types;

public abstract class ApiController : Controller
{
    protected IActionResult ToApiResponse(Result result)
    {
        if (result.IsFailure)
            return BadRequest(new ApiErrorResponse(result.FailureMessage));

        return Ok(new ApiResultResponse<bool>(true));
    }

    protected IActionResult ToApiResponse<TContent>(Result<TContent> result)
    {
        if (result.IsFailure)
            return BadRequest(new ApiErrorResponse(result.FailureMessage));

        return Ok(new ApiResultResponse<TContent>(result.Content));
    }
}
using Cerebellum.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace Cerebellum.Api.Dashboard;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController : ApiController
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    [Route("")]
    [Authenticate]
    public IActionResult GetDashboard()
    {
        var result = _dashboardService.GetDashBoard();

        return ToApiResponse(result);
    }
}
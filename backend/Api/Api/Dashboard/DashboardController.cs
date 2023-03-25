using Microsoft.AspNetCore.Mvc;
using NetApiLibs.Api;

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
    public IActionResult GetDashboard()
    {
        var result = _dashboardService.GetDashBoard();

        return ToApiResponse(result);
    }
}
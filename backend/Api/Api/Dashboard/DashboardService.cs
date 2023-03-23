using Api.Api.Dashboard.Types;
using Data.Repositories.Dashboard;
using NetApiLibs.Type;

namespace Api.Api.Dashboard;

public interface IDashboardService
{
    Result<GetDashboardResponse> GetDashBoard();
}

public sealed class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public Result<GetDashboardResponse> GetDashBoard()
    {
        var dashboard = _dashboardRepository.GetDashboard();

        return new GetDashboardResponse
        {
            Items = dashboard.Items.ConvertAll(x => new GetDashboardResponse.DashboardItem
            {
                Reference = x.Reference,
                Title = x.Title,
                Type = (GetDashboardResponse.ApiDashboardItemType)x.Type,
                CreatedAt = x.CreatedAt
            })
        };
    }
}
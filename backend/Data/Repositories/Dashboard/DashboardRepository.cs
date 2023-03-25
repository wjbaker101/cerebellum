using Data.Records;
using Data.Repositories.Dashboard.Types;

namespace Data.Repositories.Dashboard;

public interface IDashboardRepository
{
    GetDashboardDto GetDashboard(GetDashboardParameters parameters);
}

public sealed class DashboardRepository : BaseRepository, IDashboardRepository
{
    public DashboardRepository(IApiDatabase database) : base(database)
    {
    }

    public GetDashboardDto GetDashboard(GetDashboardParameters parameters)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var notes = session
            .Query<NoteRecord>()
            .OrderByDescending(x => x.CreatedAt)
            .Take(parameters.MaxItems)
            .ToList();

        var lists = session
            .Query<ListumRecord>()
            .OrderByDescending(x => x.CreatedAt)
            .Take(parameters.MaxItems)
            .ToList();

        var kanbanBoards = session
            .Query<KanbanBoardRecord>()
            .OrderByDescending(x => x.CreatedAt)
            .Take(parameters.MaxItems)
            .ToList();

        transaction.Commit();

        return new GetDashboardDto
        {
            Items = Array.Empty<GetDashboardDto.DashboardItem>()
                .Concat(notes.Select(MapItem))
                .Concat(lists.Select(MapItem))
                .Concat(kanbanBoards.Select(MapItem))
                .OrderByDescending(x => x.CreatedAt)
                .Take(parameters.MaxItems)
                .ToList()
        };
    }

    private static GetDashboardDto.DashboardItem MapItem(NoteRecord note) => new()
    {
        Reference = note.Reference,
        Title = note.Title,
        Type = DashboardItemType.Note,
        CreatedAt = note.CreatedAt
    };

    private static GetDashboardDto.DashboardItem MapItem(ListumRecord list) => new()
    {
        Reference = list.Reference,
        Title = list.Title,
        Type = DashboardItemType.List,
        CreatedAt = list.CreatedAt
    };

    private static GetDashboardDto.DashboardItem MapItem(KanbanBoardRecord kanbanBoard) => new()
    {
        Reference = kanbanBoard.Reference,
        Title = kanbanBoard.Title,
        Type = DashboardItemType.KanbanBoard,
        CreatedAt = kanbanBoard.CreatedAt
    };
}
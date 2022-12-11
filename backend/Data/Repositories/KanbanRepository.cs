using Data.Records;
using NetApiLibs.Type;
using NHibernate.Linq;

namespace Data.Repositories;

public interface IKanbanRepository
{
    KanbanBoardRecord CreateBoard(KanbanBoardRecord kanbanBoard);
    Result<KanbanBoardRecord> GetBoard(Guid reference);
}

public sealed class KanbanRepository : BaseRepository, IKanbanRepository
{
    public KanbanRepository(IApiDatabase database) : base(database)
    {
    }

    public KanbanBoardRecord CreateBoard(KanbanBoardRecord kanbanBoard) => SaveRecord(kanbanBoard);

    public Result<KanbanBoardRecord> GetBoard(Guid reference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var kanbanBoard = session
            .Query<KanbanBoardRecord>()
            .FetchMany(x => x.Columns)
            .SingleOrDefault(x => x.Reference == reference);

        session
            .Query<KanbanColumnRecord>()
            .FetchMany(x => x.Items)
            .Where(x => x.Board.Reference == reference)
            .ToFuture();

        transaction.Commit();

        if (kanbanBoard == null)
            return Result<KanbanBoardRecord>.Failure($"Kanban board '{reference}' could not be found.");

        return kanbanBoard;
    }
}
﻿using Data.Records;
using DotNetLibs.Core.Types;
using NHibernate.Linq;

namespace Data.Repositories;

public interface IKanbanRepository
{
    KanbanBoardRecord CreateBoard(KanbanBoardRecord kanbanBoard);
    KanbanBoardRecord UpdateBoard(KanbanBoardRecord kanbanBoard);
    KanbanColumnRecord CreateColumn(KanbanColumnRecord kanbanColumn);
    KanbanColumnRecord UpdateColumn(KanbanColumnRecord kanbanColumn);
    void DeleteColumn(KanbanColumnRecord kanbanColumn);
    KanbanItemRecord CreateItem(KanbanItemRecord kanbanItem);
    KanbanItemRecord UpdateItem(KanbanItemRecord kanbanItem);
    void DeleteItem(KanbanItemRecord kanbanItem);
    List<KanbanBoardRecord> GetBoards();
    Result<KanbanBoardRecord> GetBoard(Guid reference);
}

public sealed class KanbanRepository : BaseRepository, IKanbanRepository
{
    public KanbanRepository(IApiDatabase database) : base(database)
    {
    }

    public KanbanBoardRecord CreateBoard(KanbanBoardRecord kanbanBoard) => SaveRecord(kanbanBoard);
    public KanbanBoardRecord UpdateBoard(KanbanBoardRecord kanbanBoard) => UpdateRecord(kanbanBoard);

    public KanbanColumnRecord CreateColumn(KanbanColumnRecord kanbanColumn) => SaveRecord(kanbanColumn);
    public KanbanColumnRecord UpdateColumn(KanbanColumnRecord kanbanColumn) => UpdateRecord(kanbanColumn);
    public void DeleteColumn(KanbanColumnRecord kanbanColumn) => DeleteRecord(kanbanColumn);

    public KanbanItemRecord CreateItem(KanbanItemRecord kanbanItem) => SaveRecord(kanbanItem);
    public KanbanItemRecord UpdateItem(KanbanItemRecord kanbanItem) => UpdateRecord(kanbanItem);
    public void DeleteItem(KanbanItemRecord kanbanItem) => DeleteRecord(kanbanItem);

    public List<KanbanBoardRecord> GetBoards()
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var kanbanBoards = session
            .Query<KanbanBoardRecord>()
            .ToList();

        transaction.Commit();

        return kanbanBoards;
    }

    public Result<KanbanBoardRecord> GetBoard(Guid reference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<KanbanBoardRecord>()
            .FetchMany(x => x.Columns)
            .ToFuture();

        session
            .Query<KanbanColumnRecord>()
            .FetchMany(x => x.Items)
            .Where(x => x.Board.Reference == reference)
            .ToFuture();

        var kanbanBoard = query
            .SingleOrDefault(x => x.Reference == reference);

        transaction.Commit();

        if (kanbanBoard == null)
            return Result<KanbanBoardRecord>.Failure($"Kanban board '{reference}' could not be found.");

        return kanbanBoard;
    }
}
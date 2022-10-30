﻿using Data.Records;
using NetApiLibs.Type;
using NHibernate.Linq;
using System.Net;

namespace Data.Repositories;

public interface IListumRepository
{
    ListumRecord SaveList(ListumRecord note);
    ListumRecord UpdateList(ListumRecord note);
    List<ListumRecord> GetLists();
    Result<ListumRecord> GetByReference(Guid reference);
}

public sealed class ListumRepository : BaseRepository, IListumRepository
{
    public ListumRepository(IApiDatabase database) : base(database)
    {
    }

    public ListumRecord SaveList(ListumRecord note) => SaveRecord(note);
    public ListumRecord UpdateList(ListumRecord note) => UpdateRecord(note);

    public List<ListumRecord> GetLists()
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var lists = session
            .Query<ListumRecord>()
            .ToList();

        transaction.Commit();

        return lists;
    }

    public Result<ListumRecord> GetByReference(Guid reference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var list = session
            .Query<ListumRecord>()
            .FetchMany(x => x.Items)
            .SingleOrDefault(x => x.Reference == reference);

        if (list == null)
            return Result<ListumRecord>.Failure($"Unable to find list with given reference: {reference}.", HttpStatusCode.NotFound);

        transaction.Commit();

        return list;
    }
}
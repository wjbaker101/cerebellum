using Data.Records;
using NetApiLibs.Type;
using NHibernate.Linq;
using System.Net;

namespace Data.Repositories;

public interface IListumRepository
{
    Result<ListumRecord> GetByReference(Guid reference);
}

public sealed class ListumRepository : BaseRepository, IListumRepository
{
    public ListumRepository(IApiDatabase database) : base(database)
    {
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
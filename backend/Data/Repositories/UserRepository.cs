using Data.Records;
using DotNetLibs.Core.Types;
using NHibernate.Linq;

namespace Data.Repositories;

public interface IUserRepository
{
    Task<Result<UserRecord>> GetByUsername(string username, CancellationToken cancellationToken);
    Task<Result<UserRecord>> GetByReference(Guid reference, CancellationToken cancellationToken);
}

public sealed class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IApiDatabase database) : base(database)
    {
    }

    public async Task<Result<UserRecord>> GetByReference(Guid reference, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = await session
            .Query<UserRecord>()
            .SingleOrDefaultAsync(x => x.Reference == reference, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with given reference: {reference}.");

        return user;
    }

    public async Task<Result<UserRecord>> GetByUsername(string username, CancellationToken cancellationToken)
    {
        var lookup = username.ToLower();

        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = await session
            .Query<UserRecord>()
            .SingleOrDefaultAsync(x => x.Username.ToLower() == lookup, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with given username: {username}.");

        return user;
    }
}
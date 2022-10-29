namespace Data.Repositories;

public interface IListumRepository
{
}

public sealed class ListumRepository : BaseRepository, IListumRepository
{
    private readonly IApiDatabase _database;

    public ListumRepository(IApiDatabase database) : base(database)
    {
        _database = database;
    }
}
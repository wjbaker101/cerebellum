namespace Data.Repositories;

public interface IListumRepository
{
}

public sealed class ListumRepository : BaseRepository, IListumRepository
{
    public ListumRepository(IApiDatabase database) : base(database)
    {
    }
}
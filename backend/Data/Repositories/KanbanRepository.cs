namespace Data.Repositories;

public interface IKanbanRepository
{
}

public sealed class KanbanRepository : BaseRepository, IKanbanRepository
{
    public KanbanRepository(IApiDatabase database) : base(database)
    {
    }
}
namespace Api.Kanban.Types;

public sealed class UpdateBoardPositionsRequest
{
    public Dictionary<Guid, Column> Columns { get; init; } = new();

    public sealed class Column
    {
        public int Position { get; init; }
        public Dictionary<Guid, int> Items { get; init; } = new();
    }
}

public sealed class UpdateBoardPositionsResponse
{
}
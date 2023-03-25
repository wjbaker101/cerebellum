namespace Api.Api.Kanban.Types;

public sealed class UpdateBoardPositionsRequest
{
    public required Dictionary<Guid, Column> Columns { get; init; }

    public sealed class Column
    {
        public required int Position { get; init; }
        public required Dictionary<Guid, int> Items { get; init; }
    }
}

public sealed class UpdateBoardPositionsResponse
{
}
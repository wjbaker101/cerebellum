namespace Api.Listum.Types;

public sealed class ReorderListRequest
{
    public Dictionary<Guid, int> Order { get; init; } = new();
}
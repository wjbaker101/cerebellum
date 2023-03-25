namespace Cerebellum.Api.Listum.Types;

public sealed class ReorderListRequest
{
    public required Dictionary<Guid, int> Order { get; init; }
}
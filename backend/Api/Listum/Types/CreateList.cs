using Core.Model;

namespace Api.Listum.Types;

public sealed class CreateListRequest
{
    public string Title { get; init; } = null!;
}

public sealed class CreateListResponse
{
    public ListumModel List { get; init; }
}
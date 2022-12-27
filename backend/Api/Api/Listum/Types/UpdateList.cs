using Core.Model;

namespace Api.Api.Listum.Types;

public sealed class UpdateListRequest
{
    public string Title { get; init; } = null!;
}

public sealed class UpdateListResponse
{
    public ListumModel List { get; init; }
}
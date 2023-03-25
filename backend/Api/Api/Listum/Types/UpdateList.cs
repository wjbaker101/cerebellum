using Core.Model;

namespace Api.Api.Listum.Types;

public sealed class UpdateListRequest
{
    public required string Title { get; init; }
}

public sealed class UpdateListResponse
{
    public required ListumModel List { get; init; }
}
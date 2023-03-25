using Core.Model;

namespace Cerebellum.Api.Listum.Types;

public sealed class CreateListRequest
{
    public required string Title { get; init; }
}

public sealed class CreateListResponse
{
    public required ListumModel List { get; init; }
}
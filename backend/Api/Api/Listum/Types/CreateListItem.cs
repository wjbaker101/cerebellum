using Core.Model;

namespace Cerebellum.Api.Listum.Types;

public sealed class CreateListItemRequest
{
    public required string Content { get; init; }
}

public sealed class CreateListItemResponse
{
    public required ListumItemModel ListItem { get; init; }
}
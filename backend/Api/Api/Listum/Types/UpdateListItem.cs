using Core.Model;

namespace Api.Api.Listum.Types;

public sealed class UpdateListItemRequest
{
    public required string Content { get; init; }
}

public sealed class UpdateListItemResponse
{
    public required ListumItemModel ListItem { get; init; }
}
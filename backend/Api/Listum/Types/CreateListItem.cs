using Core.Model;

namespace Api.Listum.Types;

public sealed class CreateListItemRequest
{
    public string Content { get; init; } = null!;
}

public sealed class CreateListItemResponse
{
    public ListumItemModel ListItem { get; init; }
}
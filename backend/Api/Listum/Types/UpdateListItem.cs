using Core.Model;

namespace Api.Listum.Types;

public sealed class UpdateListItemRequest
{
    public string Content { get; init; } = null!;
}

public sealed class UpdateListItemResponse
{
    public ListumItemModel ListItem { get; init; }
}
using Core.Model;

namespace Api.Api.Listum.Types;

public sealed class GetListsResponse
{
    public List<ListumModel> Lists { get; init; } = new();
}
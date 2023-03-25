using Core.Model;

namespace Api.Api.Listum.Types;

public sealed class GetListsResponse
{
    public required List<ListumModel> Lists { get; init; }
}
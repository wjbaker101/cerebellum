using Core.Model;

namespace Cerebellum.Api.Listum.Types;

public sealed class GetListsResponse
{
    public required List<ListumModel> Lists { get; init; }
}
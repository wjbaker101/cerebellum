using Core.Model;

namespace Api.Listum.Types;

public sealed class GetListByReferenceResponse
{
    public ListumModel List { get; init; }
}
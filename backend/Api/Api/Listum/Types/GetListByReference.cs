using Core.Model;

namespace Api.Api.Listum.Types;

public sealed class GetListByReferenceResponse
{
    public required ListumModel List { get; init; }
}
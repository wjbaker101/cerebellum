using Core.Model;

namespace Cerebellum.Api.Listum.Types;

public sealed class GetListByReferenceResponse
{
    public required ListumModel List { get; init; }
}
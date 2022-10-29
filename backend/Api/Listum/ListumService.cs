using Api.Listum.Types;
using Core.Model;
using Data.Repositories;
using NetApiLibs.Extension;
using NetApiLibs.Type;

namespace Api.Listum;

public interface IListumService
{
    Result<GetListByReferenceResponse> GetListByReference(Guid reference);
}

public sealed class ListumService : IListumService
{
    private readonly IListumRepository _listumRepository;

    public ListumService(IListumRepository listumRepository)
    {
        _listumRepository = listumRepository;
    }

    public Result<GetListByReferenceResponse> GetListByReference(Guid reference)
    {
        var listResult = _listumRepository.GetByReference(reference);
        if (!listResult.TrySuccess(out var list))
            return Result<GetListByReferenceResponse>.FromFailure(listResult);

        return new GetListByReferenceResponse
        {
            List = new ListumModel
            {
                Reference = list.Reference,
                CreatedAt = list.CreatedAt,
                Title = list.Title,
                Items = list.Items.ConvertAll(x => new ListumItemModel
                {
                    Reference = x.Reference,
                    CreatedAt = x.CreatedAt,
                    Content = x.Content,
                    ListOrder = x.ListOrder
                })
            }
        };
    }
}
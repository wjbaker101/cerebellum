using Api.Listum.Types;
using Core.Model;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Extension;
using NetApiLibs.Type;

namespace Api.Listum;

public interface IListumService
{
    Result<GetListsResponse> GetLists();
    Result<GetListByReferenceResponse> GetListByReference(Guid reference);
    Result<CreateListResponse> CreateList(CreateListRequest request);
    Result<UpdateListResponse> UpdateList(Guid reference, UpdateListRequest request);
}

public sealed class ListumService : IListumService
{
    private readonly IListumRepository _listumRepository;

    public ListumService(IListumRepository listumRepository)
    {
        _listumRepository = listumRepository;
    }

    public Result<GetListsResponse> GetLists()
    {
        var lists = _listumRepository.GetLists();

        return new GetListsResponse
        {
            Lists = lists.ConvertAll(x => new ListumModel
            {
                Reference = x.Reference,
                CreatedAt = x.CreatedAt,
                Title = x.Title,
                Items = new List<ListumItemModel>()
            })
        };
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

    public Result<CreateListResponse> CreateList(CreateListRequest request)
    {
        var list = _listumRepository.SaveList(new ListumRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Title = request.Title
        });

        return new CreateListResponse
        {
            List = new ListumModel
            {
                Reference = list.Reference,
                CreatedAt = list.CreatedAt,
                Title = list.Title,
                Items = new List<ListumItemModel>()
            }
        };
    }

    public Result<UpdateListResponse> UpdateList(Guid reference, UpdateListRequest request)
    {
        var listResult = _listumRepository.GetByReference(reference);
        if (!listResult.TrySuccess(out var list))
            return Result<UpdateListResponse>.FromFailure(listResult);

        list.Title = request.Title;

        _listumRepository.UpdateList(list);

        return new UpdateListResponse
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
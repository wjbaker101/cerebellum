using Api.Listum.Types;
using Core.Model;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Extension;
using NetApiLibs.Type;
using System.Net;

namespace Api.Listum;

public interface IListumService
{
    Result<GetListsResponse> GetLists();
    Result<GetListByReferenceResponse> GetListByReference(Guid reference);
    Result<CreateListResponse> CreateList(CreateListRequest request);
    Result<UpdateListResponse> UpdateList(Guid reference, UpdateListRequest request);
    Result ReorderList(Guid reference, ReorderListRequest request);
    Result<CreateListItemResponse> CreateListItem(Guid listReference, CreateListItemRequest request);
    Result<UpdateListItemResponse> UpdateListItem(Guid listReference, Guid itemReference, UpdateListItemRequest request);
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

    public Result ReorderList(Guid reference, ReorderListRequest request)
    {
        var listResult = _listumRepository.GetByReference(reference);
        if (!listResult.TrySuccess(out var list))
            return Result<UpdateListResponse>.FromFailure(listResult);

        foreach (var item in list.Items)
        {
            if (!request.Order.TryGetValue(item.Reference, out var index))
                return Result.Failure($"The item '{item.Reference}' in the list has not been given.");

            item.ListOrder = index;
        }

        _listumRepository.UpdateItems(list.Items);

        return Result.Success();
    }

    public Result<CreateListItemResponse> CreateListItem(Guid listReference, CreateListItemRequest request)
    {
        var listResult = _listumRepository.GetByReference(listReference);
        if (listResult.IsFailure)
            return Result<CreateListItemResponse>.FromFailure(listResult);

        var listItem = _listumRepository.SaveItem(new ListumItemRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Content = request.Content,
            ListOrder = listResult.Value.Items.Any() ? listResult.Value.Items.Max(x => x.ListOrder) + 1 : 1,
            List = listResult.Value
        });

        return new CreateListItemResponse
        {
            ListItem = new ListumItemModel
            {
                Reference = listItem.Reference,
                CreatedAt = listItem.CreatedAt,
                Content = listItem.Content,
                ListOrder = listItem.ListOrder
            }
        };
    }

    public Result<UpdateListItemResponse> UpdateListItem(Guid listReference, Guid itemReference, UpdateListItemRequest request)
    {
        var listResult = _listumRepository.GetByReference(listReference);
        if (listResult.IsFailure)
            return Result<UpdateListItemResponse>.FromFailure(listResult);

        var listItem = listResult.Value.Items.SingleOrDefault(x => x.Reference == itemReference);
        if (listItem == null)
            return Result<UpdateListItemResponse>.Failure($"Unable to find list item with given reference: {itemReference}.", HttpStatusCode.NotFound);

        listItem.Content = request.Content;

        _listumRepository.UpdateItem(listItem);

        return new UpdateListItemResponse
        {
            ListItem = new ListumItemModel
            {
                Reference = listItem.Reference,
                CreatedAt = listItem.CreatedAt,
                Content = listItem.Content,
                ListOrder = listItem.ListOrder
            }
        };
    }
}
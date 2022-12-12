using Api.Kanban.Types;
using Core.Model;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Extension;
using NetApiLibs.Type;

namespace Api.Kanban;

public interface IKanbanService
{
    Result<GetKanbanBoardsResponse> GetBoards();
    Result<GetKanbanBoardResponse> GetBoard(Guid reference);
    Result<CreateKanbanBoardResponse> CreateKanbanBoard(CreateKanbanBoardRequest request);
    Result<AddKanbanColumnResponse> AddKanbanColumn(Guid boardReference, AddKanbanColumnRequest request);
    Result<AddKanbanItemResponse> AddKanbanItem(Guid boardReference, Guid columnReference, AddKanbanItemRequest request);
    Result<UpdateKanbanItemResponse> UpdateKanbanItem(Guid boardReference, Guid columnReference, Guid itemReference, UpdateKanbanItemRequest request);
}

public sealed class KanbanService : IKanbanService
{
    private readonly IKanbanRepository _kanbanRepository;

    public KanbanService(IKanbanRepository kanbanRepository)
    {
        _kanbanRepository = kanbanRepository;
    }

    public Result<GetKanbanBoardsResponse> GetBoards()
    {
        var kanbanBoards = _kanbanRepository.GetBoards();

        return new GetKanbanBoardsResponse
        {
            KanbanBoards = kanbanBoards.ConvertAll(x => new KanbanBoardModel
            {
                Reference = x.Reference,
                CreatedAt = x.CreatedAt,
                Title = x.Title
            })
        };
    }

    public Result<GetKanbanBoardResponse> GetBoard(Guid reference)
    {
        var kanbanBoardResult = _kanbanRepository.GetBoard(reference);
        if (!kanbanBoardResult.TrySuccess(out var kanbanBoard))
            return Result<GetKanbanBoardResponse>.FromFailure(kanbanBoardResult);

        return new GetKanbanBoardResponse
        {
            KanbanBoard = new KanbanBoardModel
            {
                Reference = kanbanBoard.Reference,
                CreatedAt = kanbanBoard.CreatedAt,
                Title = kanbanBoard.Title,
                Columns = kanbanBoard.Columns.ConvertAll(column => new KanbanColumnModel
                {
                    Reference = column.Reference,
                    CreatedAt = column.CreatedAt,
                    Title = column.Title,
                    Items = column.Items.ConvertAll(item => new KanbanItemModel
                    {
                        Reference = item.Reference,
                        CreatedAt = item.CreatedAt,
                        Content = item.Content
                    })
                })
            }
        };
    }

    public Result<CreateKanbanBoardResponse> CreateKanbanBoard(CreateKanbanBoardRequest request)
    {
        var kanbanBoard = _kanbanRepository.CreateBoard(new KanbanBoardRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Title = request.Title,
            Columns = new List<KanbanColumnRecord>()
        });

        return new CreateKanbanBoardResponse
        {
            KanbanBoard = new KanbanBoardModel
            {
                Reference = kanbanBoard.Reference,
                CreatedAt = kanbanBoard.CreatedAt,
                Title = kanbanBoard.Title,
                Columns = kanbanBoard.Columns.ConvertAll(column => new KanbanColumnModel
                {
                    Reference = column.Reference,
                    CreatedAt = column.CreatedAt,
                    Title = column.Title,
                    Items = column.Items.ConvertAll(item => new KanbanItemModel
                    {
                        Reference = item.Reference,
                        CreatedAt = item.CreatedAt,
                        Content = item.Content
                    })
                })
            }
        };
    }

    public Result<AddKanbanColumnResponse> AddKanbanColumn(Guid boardReference, AddKanbanColumnRequest request)
    {
        var kanbanBoardResult = _kanbanRepository.GetBoard(boardReference);
        if (!kanbanBoardResult.TrySuccess(out var kanbanBoard))
            return Result<AddKanbanColumnResponse>.FromFailure(kanbanBoardResult);

        var kanbanColumn = _kanbanRepository.CreateColumn(new KanbanColumnRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Board = kanbanBoard,
            Title = request.Title,
            Items = new List<KanbanItemRecord>()
        });

        return new AddKanbanColumnResponse
        {
            KanbanColumn = new KanbanColumnModel
            {
                Reference = kanbanColumn.Reference,
                CreatedAt = kanbanColumn.CreatedAt,
                Title = kanbanColumn.Title,
                Items = kanbanColumn.Items.ConvertAll(item => new KanbanItemModel
                {
                    Reference = item.Reference,
                    CreatedAt = item.CreatedAt,
                    Content = item.Content
                })
            }
        };
    }

    public Result<AddKanbanItemResponse> AddKanbanItem(Guid boardReference, Guid columnReference, AddKanbanItemRequest request)
    {
        var kanbanBoardResult = _kanbanRepository.GetBoard(boardReference);
        if (!kanbanBoardResult.TrySuccess(out var kanbanBoard))
            return Result<AddKanbanItemResponse>.FromFailure(kanbanBoardResult);

        var kanbanColumn = kanbanBoard.Columns.SingleOrDefault(x => x.Reference == columnReference);
        if (kanbanColumn == null)
            return Result<AddKanbanItemResponse>.Failure($"Kanban column '{columnReference}' could not be found.");

        var kanbanItem = _kanbanRepository.CreateItem(new KanbanItemRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Content = request.Content,
            Column = kanbanColumn
        });

        return new AddKanbanItemResponse
        {
            KanbanItem = new KanbanItemModel
            {
                Reference = kanbanItem.Reference,
                CreatedAt = kanbanItem.CreatedAt,
                Content = kanbanItem.Content
            }
        };
    }

    public Result<UpdateKanbanItemResponse> UpdateKanbanItem(Guid boardReference, Guid columnReference, Guid itemReference, UpdateKanbanItemRequest request)
    {
        var kanbanBoardResult = _kanbanRepository.GetBoard(boardReference);
        if (!kanbanBoardResult.TrySuccess(out var kanbanBoard))
            return Result<UpdateKanbanItemResponse>.FromFailure(kanbanBoardResult);

        var previousKanbanColumn = kanbanBoard.Columns.SingleOrDefault(x => x.Reference == columnReference);

        var kanbanItem = previousKanbanColumn?.Items.SingleOrDefault(x => x.Reference == itemReference);
        if (kanbanItem == null)
            return Result<UpdateKanbanItemResponse>.Failure($"Kanban item '{itemReference}' in column '{columnReference}' could not be found.");

        var newKanbanColumn = kanbanBoard.Columns.SingleOrDefault(x => x.Reference == request.ColumnReference);
        if (newKanbanColumn == null)
            return Result<UpdateKanbanItemResponse>.Failure($"Kanban column '{request.ColumnReference}' could not be found.");

        kanbanItem.Content = request.Content;
        kanbanItem.Column = newKanbanColumn;

        _kanbanRepository.UpdateItem(kanbanItem);

        return new UpdateKanbanItemResponse
        {
            KanbanItem = new KanbanItemModel
            {
                Reference = kanbanItem.Reference,
                CreatedAt = kanbanItem.CreatedAt,
                Content = kanbanItem.Content
            }
        };
    }
}
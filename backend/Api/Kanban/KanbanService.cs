using Api.Kanban.Types;
using Core.Model;
using Data.Mappers;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Type;

namespace Api.Kanban;

public interface IKanbanService
{
    Result<GetKanbanBoardsResponse> GetBoards();
    Result<GetKanbanBoardResponse> GetBoard(Guid reference);
    Result<CreateKanbanBoardResponse> CreateKanbanBoard(CreateKanbanBoardRequest request);
    Result<UpdateBoardPositionsResponse> UpdateBoardPositions(Guid boardReference, UpdateBoardPositionsRequest request);
    Result<AddKanbanColumnResponse> AddKanbanColumn(Guid boardReference, AddKanbanColumnRequest request);
    Result<AddKanbanItemResponse> AddKanbanItem(Guid boardReference, Guid columnReference, AddKanbanItemRequest request);
    Result<UpdateKanbanItemResponse> UpdateKanbanItem(Guid boardReference, Guid columnReference, Guid itemReference, UpdateKanbanItemRequest request);
    Result DeleteKanbanItem(Guid boardReference, Guid columnReference, Guid itemReference);
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
            KanbanBoard = KanbanMapper.MapBoard(kanbanBoard)
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
            KanbanBoard = KanbanMapper.MapBoard(kanbanBoard)
        };
    }

    public Result<UpdateBoardPositionsResponse> UpdateBoardPositions(Guid boardReference, UpdateBoardPositionsRequest request)
    {
        var kanbanBoardResult = _kanbanRepository.GetBoard(boardReference);
        if (!kanbanBoardResult.TrySuccess(out var kanbanBoard))
            return Result<UpdateBoardPositionsResponse>.FromFailure(kanbanBoardResult);

        var columnDictionary = kanbanBoard.Columns.ToDictionary(x => x.Reference, x => x);

        foreach (var column in kanbanBoard.Columns)
        {
            column.Position = request.Columns[column.Reference].Position;

            _kanbanRepository.UpdateColumn(column);
        }

        foreach (var item in kanbanBoard.Columns.SelectMany(x => x.Items))
        {
            var column = request.Columns.SingleOrDefault(col => col.Value.Items.ContainsKey(item.Reference));

            item.Position = column.Value.Items[item.Reference];

            item.Column = columnDictionary[column.Key];

            _kanbanRepository.UpdateItem(item);
        }

        return new UpdateBoardPositionsResponse();
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
            Position = kanbanBoard.Columns.Max(x => x.Position) + 1,
            Items = new List<KanbanItemRecord>()
        });

        return new AddKanbanColumnResponse
        {
            KanbanColumn = KanbanMapper.MapColumn(kanbanColumn)
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
            Position = kanbanColumn.Items.Max(x => x.Position) + 1,
            Column = kanbanColumn
        });

        return new AddKanbanItemResponse
        {
            KanbanItem = KanbanMapper.MapItem(kanbanItem)
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
            KanbanItem = KanbanMapper.MapItem(kanbanItem)
        };
    }

    public Result DeleteKanbanItem(Guid boardReference, Guid columnReference, Guid itemReference)
    {
        var kanbanBoardResult = _kanbanRepository.GetBoard(boardReference);
        if (!kanbanBoardResult.TrySuccess(out var kanbanBoard))
            return Result<UpdateKanbanItemResponse>.FromFailure(kanbanBoardResult);

        var kanbanColumn = kanbanBoard.Columns.SingleOrDefault(x => x.Reference == columnReference);
        if (kanbanColumn == null)
            return Result.Failure($"Kanban column '{columnReference}' could not be found.");

        var kanbanItem = kanbanColumn.Items.SingleOrDefault(x => x.Reference == itemReference);
        if (kanbanItem == null)
            return Result.Failure($"Kanban item '{itemReference}' could not be found.");

        _kanbanRepository.DeleteItem(kanbanItem);

        return Result.Success();
    }
}
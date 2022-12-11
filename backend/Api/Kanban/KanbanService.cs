using Api.Kanban.Types;
using Core.Model;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Extension;
using NetApiLibs.Type;

namespace Api.Kanban;

public interface IKanbanService
{
    Result<GetKanbanBoardResponse> GetBoard(Guid reference);
    Result<CreateKanbanBoardResponse> CreateKanbanBoard(CreateKanbanBoardRequest request);
}

public sealed class KanbanService : IKanbanService
{
    private readonly IKanbanRepository _kanbanRepository;

    public KanbanService(IKanbanRepository kanbanRepository)
    {
        _kanbanRepository = kanbanRepository;
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
}
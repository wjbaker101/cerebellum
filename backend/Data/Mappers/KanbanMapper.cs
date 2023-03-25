using Core.Model;
using Data.Records;
using NetApiLibs.Extension;

namespace Data.Mappers;

public static class KanbanMapper
{
    public static KanbanBoardModel MapBoard(KanbanBoardRecord board) => new()
    {
        Reference = board.Reference,
        CreatedAt = board.CreatedAt,
        Title = board.Title,
        Columns = board.Columns.OrderBy(x => x.Position).ConvertAll(MapColumn)
    };

    public static KanbanColumnModel MapColumn(KanbanColumnRecord column) => new()
    {
        Reference = column.Reference,
        CreatedAt = column.CreatedAt,
        Title = column.Title,
        Position = column.Position,
        Items = column.Items.OrderBy(x => x.Position).ConvertAll(MapItem)
    };

    public static KanbanItemModel MapItem(KanbanItemRecord item) => new()
    {
        Reference = item.Reference,
        CreatedAt = item.CreatedAt,
        Content = item.Content,
        Position = item.Position
    };
}
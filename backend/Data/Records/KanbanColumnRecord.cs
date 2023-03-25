using FluentNHibernate.Mapping;

namespace Data.Records;

public class KanbanColumnRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Title { get; set; }
    public virtual required int Position { get; set; }
    public virtual required ICollection<KanbanItemRecord> Items { get; set; }
    public virtual required KanbanBoardRecord Board { get; init; }
}

public sealed class KanbanColumnRecordMap : ClassMap<KanbanColumnRecord>
{
    public KanbanColumnRecordMap()
    {
        Schema("cerebellum");
        Table("kanban_column");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("kanban_column_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Title, "title");
        Map(x => x.Position, "position");
        HasMany(x => x.Items).KeyColumn("kanban_column_id").Inverse().AsSet();
        References(x => x.Board, "kanban_board_id");
    }
}
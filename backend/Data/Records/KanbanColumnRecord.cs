using FluentNHibernate.Mapping;

namespace Data.Records;

public class KanbanColumnRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual string Title { get; set; } = null!;
    public virtual ICollection<KanbanItemRecord> Items { get; set; } = new HashSet<KanbanItemRecord>();
    public virtual KanbanBoardRecord Board { get; init; } = null!;
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
        HasMany(x => x.Items).KeyColumn("kanban_item_id").Inverse().AsSet();
        References(x => x.Board, "kanban_board_id");
    }
}
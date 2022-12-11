using FluentNHibernate.Mapping;

namespace Data.Records;

public class KanbanBoardRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual string Title { get; set; } = null!;
    public virtual ICollection<KanbanColumnRecord> Columns { get; set; } = new HashSet<KanbanColumnRecord>();
}

public sealed class KanbanBoardRecordMap : ClassMap<KanbanBoardRecord>
{
    public KanbanBoardRecordMap()
    {
        Schema("cerebellum");
        Table("kanban_board");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("kanban_board_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Title, "title");
        HasMany(x => x.Columns).KeyColumn("kanban_board_id").Inverse().AsSet();
    }
}
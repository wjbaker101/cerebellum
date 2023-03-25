using FluentNHibernate.Mapping;

namespace Data.Records;

public class KanbanItemRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Content { get; set; }
    public virtual required int Position { get; set; }
    public virtual required KanbanColumnRecord Column { get; set; }
}

public sealed class KanbanItemRecordMap : ClassMap<KanbanItemRecord>
{
    public KanbanItemRecordMap()
    {
        Schema("cerebellum");
        Table("kanban_item");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("kanban_item_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Content, "content");
        Map(x => x.Position, "position");
        References(x => x.Column, "kanban_column_id");
    }
}
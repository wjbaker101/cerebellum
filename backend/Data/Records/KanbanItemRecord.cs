using FluentNHibernate.Mapping;

namespace Data.Records;

public class KanbanItemRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual string Content { get; set; } = null!;
    public virtual KanbanColumnRecord Column { get; init; } = null!;
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
        References(x => x.Column, "kanban_column_id");
    }
}
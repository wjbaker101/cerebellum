using FluentNHibernate.Mapping;

namespace Data.Records;

public class ListumItemRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual string Content { get; set; } = null!;
    public virtual int ListOrder { get; set; }
    public virtual ListumRecord List { get; init; } = null!;
}

public sealed class ListumItemRecordMap : ClassMap<ListumItemRecord>
{
    public ListumItemRecordMap()
    {
        Schema("cerebellum");
        Table("listum_item");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("listum_item_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Content, "content");
        Map(x => x.ListOrder, "list_order");
        References(x => x.List);
    }
}
using FluentNHibernate.Mapping;

namespace Data.Records;

public class ListumItemRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Content { get; set; }
    public virtual required int ListOrder { get; set; }
    public virtual required ListumRecord List { get; init; }
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
        References(x => x.List).Column("listum_id");
    }
}
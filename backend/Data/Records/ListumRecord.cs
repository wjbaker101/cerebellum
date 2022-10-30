using FluentNHibernate.Mapping;

namespace Data.Records;

public class ListumRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual string Title { get; set; } = null!;
    public virtual ISet<ListumItemRecord> Items { get; init; } = new HashSet<ListumItemRecord>();
}

public sealed class ListumRecordMap : ClassMap<ListumRecord>
{
    public ListumRecordMap()
    {
        Schema("cerebellum");
        Table("listum");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("listum_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Title, "title");
        HasMany(x => x.Items).KeyColumn("listum_id");
    }
}
using FluentNHibernate.Mapping;

namespace Data.Records;

public class NoteRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; init; }
    public virtual DateTime CreatedAt { get; init; }
    public virtual string Content { get; set; } = null!;
}

public sealed class NoteRecordMap : ClassMap<NoteRecord>
{
    public NoteRecordMap()
    {
        Schema("notes");
        Table("note");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("note_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Content, "content");
    }
}
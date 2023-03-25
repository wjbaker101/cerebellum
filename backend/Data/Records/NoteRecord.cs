using FluentNHibernate.Mapping;

namespace Data.Records;

public class NoteRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Title { get; set; }
    public virtual required string Content { get; set; } 
}

public sealed class NoteRecordMap : ClassMap<NoteRecord>
{
    public NoteRecordMap()
    {
        Schema("cerebellum");
        Table("note");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("note_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Title, "title");
        Map(x => x.Content, "content");
    }
}
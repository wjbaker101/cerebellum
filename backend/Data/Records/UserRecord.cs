using FluentNHibernate.Mapping;

namespace Data.Records;

public class UserRecord
{
    public virtual required long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Username { get; set; }
    public virtual required string Password { get; set; }
    public virtual required string PasswordSalt { get; set; }
}

public sealed class UserRecordMap : ClassMap<UserRecord>
{
    public UserRecordMap()
    {
        Schema("cerebellum");
        Table("user");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("cerebellum.user_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Username, "username");
        Map(x => x.Password, "password");
        Map(x => x.PasswordSalt, "password_salt");
    }
}
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Data;

public interface IApiDatabase
{
    ISessionFactory SessionFactory { get; }
}

public sealed class ApiDatabase : IApiDatabase
{
    public ISessionFactory SessionFactory { get; }

    public ApiDatabase()
    {
        SessionFactory = Fluently.Configure()
            .Database(PostgreSQLConfiguration.Standard.ConnectionString(c => c
                .Host("localhost")
                .Port(5432)
                .Database("cerebellum")
                .Username("postgres")
                .Password("password")))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ApiDatabase>())
            .BuildSessionFactory();
    }
}
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
            .Database(PostgreSQLConfiguration.Standard.ConnectionString(Environment.GetEnvironmentVariable("DOTNET_PG_CONNECTION_STRING")))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ApiDatabase>())
            .BuildSessionFactory();
    }
}
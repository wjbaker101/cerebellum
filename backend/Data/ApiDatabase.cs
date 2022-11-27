using Core.Settings;
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

    public ApiDatabase(AppSecrets appSecrets)
    {
        SessionFactory = Fluently.Configure()
            .Database(PostgreSQLConfiguration.Standard.ConnectionString(c => c
                .Host(appSecrets.Database.Host)
                .Port(appSecrets.Database.Port)
                .Database(appSecrets.Database.Database)
                .Username(appSecrets.Database.Username)
                .Password(appSecrets.Database.Password)))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ApiDatabase>())
            .BuildSessionFactory();
    }
}
using Moq;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Data.Tests._Helper;

public sealed class DatabaseHelper
{
    private readonly Mock<ISession> _session;
    private readonly Mock<IApiDatabase> _database;

    private DatabaseHelper()
    {
        _session = new Mock<ISession>();
        _session
            .Setup(mock => mock.BeginTransaction())
            .Returns(Moq.Mock.Of<ITransaction>());

        _database = new Mock<IApiDatabase>();
        _database
            .Setup(mock => mock.SessionFactory.OpenSession())
            .Returns(_session.Object);
    }

    public static Mock<IApiDatabase> Mock<T>(List<T> records)
    {
        var helper = new DatabaseHelper();
        helper
            ._session
            .Setup(mock => mock.Query<T>())
            .Returns(records.AsQueryable());

        return helper._database;
    }
}
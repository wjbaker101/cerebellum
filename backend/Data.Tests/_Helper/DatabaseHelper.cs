using Moq;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Data.Tests._Helper;

public static class DatabaseHelper
{
    private static readonly Mock<ISession> Session;
    private static readonly Mock<IApiDatabase> Database;

    static DatabaseHelper()
    {
        Session = new Mock<ISession>();
        Session
            .Setup(mock => mock.BeginTransaction())
            .Returns(Moq.Mock.Of<ITransaction>());

        Database = new Mock<IApiDatabase>();
        Database
            .Setup(mock => mock.SessionFactory.OpenSession())
            .Returns(Session.Object);
    }

    public static Mock<IApiDatabase> Mock<T>(List<T> records)
    {
        Session
            .Setup(mock => mock.Query<T>())
            .Returns(records.AsQueryable());

        return Database;
    }
}
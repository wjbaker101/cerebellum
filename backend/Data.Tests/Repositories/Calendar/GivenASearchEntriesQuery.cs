using Data.Records;
using Data.Repositories;
using Moq;
using NHibernate;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Tests.Repositories.Calendar;

[TestFixture]
[Parallelizable]
public sealed class GivenASearchEntriesQuery
{
    private List<CalendarEntryRecord> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var records = new List<CalendarEntryRecord>
        {
            new()
            {
                Reference = Guid.Parse("6fde403b-b32e-42e5-8ac5-7785b23b6d62"),
                StartAt = new DateTime(2020, 04, 03),
                EndAt = new DateTime(2020, 04, 07)
            }
        };

        var session = new Mock<ISession>();
        session
            .Setup(mock => mock.BeginTransaction())
            .Returns(Mock.Of<ITransaction>());
        session
            .Setup(mock => mock.Query<CalendarEntryRecord>())
            .Returns(records.AsQueryable());

        var database = new Mock<IApiDatabase>();
        database
            .Setup(mock => mock.SessionFactory.OpenSession())
            .Returns(session.Object);

        var subject = new CalendarRepository(database.Object);

        _result = subject.SearchEntries(new DateTime(2020, 04, 04), new DateTime(2020, 04, 06));
    }

    [Test]
    public void ThenTheCorrectNumberOfEntriesAreReturned()
    {
        Assert.That(_result.Count, Is.EqualTo(1));
    }

    [Test]
    public void ThenTheCorrectEntryIsReturned()
    {
        var entry = _result.First();

        Assert.That(entry.Reference, Is.EqualTo(Guid.Parse("6fde403b-b32e-42e5-8ac5-7785b23b6d62")));
    }
}
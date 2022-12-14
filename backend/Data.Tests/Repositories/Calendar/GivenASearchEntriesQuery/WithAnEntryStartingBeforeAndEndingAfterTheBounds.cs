using Data.Records;
using Data.Repositories;
using Data.Tests._Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Tests.Repositories.Calendar.GivenASearchEntriesQuery;

[TestFixture]
[Parallelizable]
public sealed class WithAnEntryStartingBeforeAndEndingAfterTheBounds
{
    private List<CalendarEntryRecord> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var database = DatabaseHelper.Mock(new List<CalendarEntryRecord>
        {
            new()
            {
                Reference = Guid.Parse("6fde403b-b32e-42e5-8ac5-7785b23b6d62"),
                StartAt = new DateTime(2020, 04, 03),
                EndAt = new DateTime(2020, 04, 07),
                RecurringPeriod = CalendarEntryRecurringPeriod.None
            }
        });

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
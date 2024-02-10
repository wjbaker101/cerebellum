using Data.Records;
using Data.Repositories;
using Data.Tests._Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Data.Tests.Repositories.Calendar.GivenASearchEntriesQuery;

[TestFixture]
[Parallelizable]
public sealed class WithAnEntryWithARecurringPeriod
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
                CreatedAt = new DateTime(2006, 04, 22, 15, 09, 01),
                Description = "TestDescription",
                StartAt = new DateTime(2020, 04, 03),
                EndAt = new DateTime(2020, 04, 03),
                RecurringPeriod = CalendarEntryRecurringPeriod.Weekly
            },
            new()
            {
                Reference = Guid.Parse("08b07415-05de-4ff5-90ab-45558f786b23"),
                CreatedAt = new DateTime(2006, 04, 22, 15, 09, 01),
                Description = "TestDescription",
                StartAt = new DateTime(2020, 04, 03),
                EndAt = new DateTime(2020, 04, 03),
                RecurringPeriod = CalendarEntryRecurringPeriod.Monthly
            },
            new()
            {
                Reference = Guid.Parse("e88fbba6-6286-4120-813d-691cb2558c67"),
                CreatedAt = new DateTime(2006, 04, 22, 15, 09, 01),
                Description = "TestDescription",
                StartAt = new DateTime(2020, 04, 03),
                EndAt = new DateTime(2020, 04, 03),
                RecurringPeriod = CalendarEntryRecurringPeriod.Yearly
            }
        });

        var subject = new CalendarRepository(database.Object);

        _result = subject.SearchEntries(new DateTime(2020, 04, 04), new DateTime(2020, 04, 06));
    }

    [Test]
    public void ThenTheCorrectNumberOfEntriesAreReturned()
    {
        Assert.That(_result.Count, Is.EqualTo(3));
    }

    [Test]
    public void ThenTheCorrectEntriesAreReturned()
    {
        Assert.That(_result[0].Reference, Is.EqualTo(Guid.Parse("6fde403b-b32e-42e5-8ac5-7785b23b6d62")));
        Assert.That(_result[1].Reference, Is.EqualTo(Guid.Parse("08b07415-05de-4ff5-90ab-45558f786b23")));
        Assert.That(_result[2].Reference, Is.EqualTo(Guid.Parse("e88fbba6-6286-4120-813d-691cb2558c67")));
    }
}
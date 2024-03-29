﻿using Cerebellum.Api.Notes;
using DotNetLibs.Core.Types;
using NUnit.Framework;
using System;
using TestHelpers.Fakes;

namespace Api.Tests.Notes;

[TestFixture]
[Parallelizable]
public sealed class GivenADeleteNoteRequestThatFailsToGetNote
{
    private Result _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var subject = new NotesService(FakeNotesRepository.Failure("TestFailure"));

        _result = subject.DeleteNote(Guid.Parse("f1f7ddb9-fd5e-4c5e-a4a4-eed1dc875d50"));
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("TestFailure"));
    }
}
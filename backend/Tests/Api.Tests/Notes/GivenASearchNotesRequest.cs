﻿using Cerebellum.Api.Notes;
using Cerebellum.Api.Notes.Types;
using DotNetLibs.Core.Types;
using NUnit.Framework;
using System;
using TestHelpers.Fakes;

namespace Api.Tests.Notes;

[TestFixture]
[Parallelizable]
public sealed class GivenASearchNotesRequest
{
    private Result<SearchNotesResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var subject = new NotesService(FakeNotesRepository.Default());

        _result = subject.SearchNotes();
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectNotesAreReturned()
    {
        var note = _result.Content.Notes[0];

        Assert.Multiple(() =>
        {
            Assert.That(note.Reference, Is.EqualTo(Guid.Parse("8be11292-8788-4a8a-b3e4-93835086a4b2")), nameof(note.Reference));
            Assert.That(note.CreatedAt, Is.EqualTo(new DateTime(2022, 05, 14, 00, 05, 42)), nameof(note.CreatedAt));
            Assert.That(note.Title, Is.EqualTo("TestTitle"), nameof(note.Title));
            Assert.That(note.Content, Is.EqualTo("TestContent"), nameof(note.Content));
        });
    }
}
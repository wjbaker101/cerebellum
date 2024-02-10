using Cerebellum.Api.Notes;
using Cerebellum.Api.Notes.Types;
using DotNetLibs.Core.Types;
using NUnit.Framework;
using System;
using TestHelpers.Fakes;

namespace Api.Tests.Notes;

[TestFixture]
[Parallelizable]
public sealed class GivenACreateNoteRequest
{
    private FakeNotesRepository _notesRepository = null!;

    private Result<CreateNoteResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _notesRepository = FakeNotesRepository.Default();

        var subject = new NotesService(_notesRepository);

        _result = subject.CreateNote(new CreateNoteRequest
        {
            Title = "TestTitle",
            Content = "TestContent"
        });
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheNoteIsSavedCorrectly()
    {
        _notesRepository.AssertSaved();

        var note = _notesRepository.ActionedNote!;

        Assert.That(note.Reference, Is.Not.EqualTo(default(Guid)));
        Assert.That(note.CreatedAt, Is.Not.EqualTo(default(DateTime)));
        Assert.That(note.Title, Is.EqualTo("TestTitle"));
        Assert.That(note.Content, Is.EqualTo("TestContent"));
    }

    [Test]
    public void ThenTheCorrectNotesAreReturned()
    {
        var note = _result.Content.Note;

        Assert.Multiple(() =>
        {
            Assert.That(note.Title, Is.EqualTo("TestTitle"), nameof(note.Title));
            Assert.That(note.Content, Is.EqualTo("TestContent"), nameof(note.Content));
        });
    }
}
using Cerebellum.Api.Notes;
using Cerebellum.Api.Notes.Types;
using NetApiLibs.Type;
using NUnit.Framework;
using System;
using TestHelpers.Fakes;

namespace Api.Tests.Notes;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdateNoteRequest
{
    private FakeNotesRepository _notesRepository = null!;

    private Result<UpdateNoteResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _notesRepository = FakeNotesRepository.Default();

        var subject = new NotesService(_notesRepository);

        _result = subject.UpdateNote(Guid.Parse("f1f7ddb9-fd5e-4c5e-a4a4-eed1dc875d50"), new UpdateNoteRequest
        {
            Title = "TestTitle2",
            Content = "TestContent2"
        });
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheNoteIsUpdatedCorrectly()
    {
        _notesRepository.AssertUpdated();

        var note = _notesRepository.ActionedNote!;

        Assert.That(note.Title, Is.EqualTo("TestTitle2"));
        Assert.That(note.Content, Is.EqualTo("TestContent2"));
    }

    [Test]
    public void ThenTheCorrectNoteIsReturned()
    {
        var note = _result.Value.Note;

        Assert.Multiple(() =>
        {
            Assert.That(note.Title, Is.EqualTo("TestTitle2"), nameof(note.Title));
            Assert.That(note.Content, Is.EqualTo("TestContent2"), nameof(note.Content));
        });
    }
}
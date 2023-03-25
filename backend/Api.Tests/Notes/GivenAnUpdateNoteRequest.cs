using Api.Tests.Notes._Helper;
using Cerebellum.Api.Notes;
using Cerebellum.Api.Notes.Types;
using Data.Records;
using Data.Repositories;
using Moq;
using NetApiLibs.Type;
using NUnit.Framework;
using System;

namespace Api.Tests.Notes;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdateNoteRequest
{
    private NoteRecord _note = null!;

    private Mock<INotesRepository> _notesRepository = null!;

    private Result<UpdateNoteResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _note = TestNote.Get();

        _notesRepository = new Mock<INotesRepository>();
        _notesRepository
            .Setup(mock => mock.GetByReference(It.IsAny<Guid>()))
            .Returns(_note);

        var subject = new NotesService(_notesRepository.Object);

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
        _notesRepository.Verify(mock => mock.UpdateNote(It.Is<NoteRecord>(request =>
            request.Reference == _note.Reference &&
            request.CreatedAt == _note.CreatedAt &&
            request.Title == "TestTitle2" &&
            request.Content == "TestContent2")), Times.Once);
    }

    [Test]
    public void ThenTheCorrectNotesAreReturned()
    {
        var note = _result.Value.Note;

        Assert.Multiple(() =>
        {
            Assert.That(note.Reference, Is.EqualTo(_note.Reference), nameof(note.Reference));
            Assert.That(note.CreatedAt, Is.EqualTo(_note.CreatedAt), nameof(note.CreatedAt));
            Assert.That(note.Title, Is.EqualTo("TestTitle2"), nameof(note.Title));
            Assert.That(note.Content, Is.EqualTo("TestContent2"), nameof(note.Content));
        });
    }
}
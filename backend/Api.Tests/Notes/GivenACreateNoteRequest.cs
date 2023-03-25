using Cerebellum.Api.Notes;
using Cerebellum.Api.Notes.Types;
using Data.Records;
using Data.Repositories;
using Moq;
using NetApiLibs.Type;
using NUnit.Framework;

namespace Api.Tests.Notes;

[TestFixture]
[Parallelizable]
public sealed class GivenACreateNoteRequest
{
    private Mock<INotesRepository> _notesRepository = null!;

    private Result<CreateNoteResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _notesRepository = new Mock<INotesRepository>();
        _notesRepository
            .Setup(mock => mock.SaveNote(It.IsAny<NoteRecord>()))
            .Returns((NoteRecord note) => note);

        var subject = new NotesService(_notesRepository.Object);

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
        _notesRepository.Verify(mock => mock.SaveNote(It.Is<NoteRecord>(request =>
            request.Reference != default &&
            request.CreatedAt != default &&
            request.Title == "TestTitle" &&
            request.Content == "TestContent")), Times.Once);
    }

    [Test]
    public void ThenTheCorrectNotesAreReturned()
    {
        var note = _result.Value.Note;

        Assert.Multiple(() =>
        {
            Assert.That(note.Title, Is.EqualTo("TestTitle"), nameof(note.Title));
            Assert.That(note.Content, Is.EqualTo("TestContent"), nameof(note.Content));
        });
    }
}
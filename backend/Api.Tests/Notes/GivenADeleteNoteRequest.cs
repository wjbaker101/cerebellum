using Api.Tests.Notes._Helper;
using Cerebellum.Api.Notes;
using Data.Records;
using Data.Repositories;
using Moq;
using NetApiLibs.Type;
using NUnit.Framework;
using System;

namespace Api.Tests.Notes;

[TestFixture]
[Parallelizable]
public sealed class GivenADeleteNoteRequest
{
    private NoteRecord _note = null!;

    private Mock<INotesRepository> _notesRepository = null!;

    private Result _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _note = TestNote.Get();

        _notesRepository = new Mock<INotesRepository>();
        _notesRepository
            .Setup(mock => mock.GetByReference(It.IsAny<Guid>()))
            .Returns(_note);

        var subject = new NotesService(_notesRepository.Object);

        _result = subject.DeleteNote(Guid.Parse("f1f7ddb9-fd5e-4c5e-a4a4-eed1dc875d50"));
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheNoteIsDeleted()
    {
        _notesRepository.Verify(mock => mock.DeleteNote(_note), Times.Once);
    }
}
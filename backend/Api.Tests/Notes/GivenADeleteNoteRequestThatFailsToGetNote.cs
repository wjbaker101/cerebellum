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
public sealed class GivenADeleteNoteRequestThatFailsToGetNote
{
    private Mock<INotesRepository> _notesRepository = null!;

    private Result _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _notesRepository = new Mock<INotesRepository>();
        _notesRepository
            .Setup(mock => mock.GetByReference(It.IsAny<Guid>()))
            .Returns(Result<NoteRecord>.Failure("TestFailure"));

        var subject = new NotesService(_notesRepository.Object);

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
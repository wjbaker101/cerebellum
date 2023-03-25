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
public sealed class GivenAnUpdateNoteRequestThatFailsToGetNote
{
    private Result<UpdateNoteResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var notesRepository = new Mock<INotesRepository>();
        notesRepository
            .Setup(mock => mock.GetByReference(It.IsAny<Guid>()))
            .Returns(Result<NoteRecord>.Failure("TestFailure"));

        var subject = new NotesService(notesRepository.Object);

        _result = subject.UpdateNote(Guid.Parse("f1f7ddb9-fd5e-4c5e-a4a4-eed1dc875d50"), new UpdateNoteRequest
        {
            Title = "TestTitle",
            Content = "TestContent"
        });
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
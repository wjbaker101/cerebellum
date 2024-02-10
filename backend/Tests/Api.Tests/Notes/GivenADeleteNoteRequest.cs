using Cerebellum.Api.Notes;
using DotNetLibs.Core.Types;
using NUnit.Framework;
using System;
using TestHelpers.Fakes;

namespace Api.Tests.Notes;

[TestFixture]
[Parallelizable]
public sealed class GivenADeleteNoteRequest
{
    private FakeNotesRepository _notesRepository = null!;

    private Result _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _notesRepository = FakeNotesRepository.Default();

        var subject = new NotesService(_notesRepository);

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
        _notesRepository.AssertDeleted();
    }
}
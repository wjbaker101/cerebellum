using Data.Records;
using Data.Repositories;
using NetApiLibs.Type;
using NUnit.Framework;

namespace TestHelpers.Fakes;

public sealed class FakeNotesRepository : INotesRepository
{
    public NoteRecord? ActionedNote { get; private set; }
    private bool _isSaved;
    private bool _isUpdated;
    private bool _isDeleted;

    private readonly Result<NoteRecord> _noteResult;

    private FakeNotesRepository(Result<NoteRecord> noteResult)
    {
        _noteResult = noteResult;
    }

    public static FakeNotesRepository Default() => new(new NoteRecord
    {
        Reference = Guid.Parse("8be11292-8788-4a8a-b3e4-93835086a4b2"),
        CreatedAt = new DateTime(2022, 05, 14, 00, 05, 42),
        Title = "TestTitle",
        Content = "TestContent"
    });

    public static FakeNotesRepository Failure(string failure) => new(Result<NoteRecord>.Failure(failure));

    public void AssertSaved() => Assert.That(_isSaved, Is.True);
    public void AssertUpdated() => Assert.That(_isUpdated, Is.True);
    public void AssertDeleted() => Assert.That(_isDeleted, Is.True);

    public NoteRecord SaveNote(NoteRecord note)
    {
        _isSaved = true;
        return ActionedNote = note;
    }

    public NoteRecord UpdateNote(NoteRecord note)
    {
        _isUpdated = true;
        return ActionedNote = note;
    }

    public List<NoteRecord> SearchNotes()
    {
        return new List<NoteRecord> { _noteResult.Value };
    }

    public Result<NoteRecord> GetByReference(Guid reference)
    {
        return _noteResult;
    }

    public void DeleteNote(NoteRecord note)
    {
        _isDeleted = true;
        ActionedNote = note;
    }
}
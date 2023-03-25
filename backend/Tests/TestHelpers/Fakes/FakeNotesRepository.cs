using Data.Records;
using Data.Repositories;
using NetApiLibs.Type;

namespace TestHelpers.Fakes;

public sealed class FakeNotesRepository : INotesRepository
{
    public NoteRecord? ActionedNote { get; private set; }

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

    public NoteRecord SaveNote(NoteRecord note)
    {
        return ActionedNote = note;
    }

    public NoteRecord UpdateNote(NoteRecord note)
    {
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
        ActionedNote = note;
    }
}
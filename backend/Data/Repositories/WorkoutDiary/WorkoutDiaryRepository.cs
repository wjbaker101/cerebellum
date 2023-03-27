using Data.Records;
using Data.Repositories.WorkoutDiary.Types;
using NetApiLibs.Type;
using NHibernate.Linq;

namespace Data.Repositories.WorkoutDiary;

public interface IWorkoutDiaryRepository
{
    WorkoutEntryRecord SaveEntry(WorkoutEntryRecord entry);
    WorkoutEntryRecord UpdateEntry(WorkoutEntryRecord entry);
    void DeleteEntry(WorkoutEntryRecord entry);
    WorkoutEntryExerciseRecord SaveExercise(WorkoutEntryExerciseRecord exercise);
    WorkoutEntryExerciseRecord UpdateExercise(WorkoutEntryExerciseRecord exercise);
    void DeleteExercise(WorkoutEntryExerciseRecord exercise);
    WorkoutEntrySetRecord SaveSet(WorkoutEntrySetRecord exercise);
    WorkoutEntrySetRecord UpdateSet(WorkoutEntrySetRecord exercise);
    void DeleteSet(WorkoutEntrySetRecord exercise);
    SearchEntriesDto SearchEntries(SearchEntriesParameters parameters);
    Result<WorkoutEntryRecord> GetEntryByReference(Guid reference);
    List<WorkoutEntryRecord> GetEntries();
}

public sealed class WorkoutDiaryRepository : BaseRepository, IWorkoutDiaryRepository
{
    public WorkoutDiaryRepository(IApiDatabase database) : base(database)
    {
    }

    public WorkoutEntryRecord SaveEntry(WorkoutEntryRecord entry) => SaveRecord(entry);
    public WorkoutEntryRecord UpdateEntry(WorkoutEntryRecord entry) => UpdateRecord(entry);
    public void DeleteEntry(WorkoutEntryRecord entry) => DeleteRecord(entry);

    public WorkoutEntryExerciseRecord SaveExercise(WorkoutEntryExerciseRecord exercise) => SaveRecord(exercise);
    public WorkoutEntryExerciseRecord UpdateExercise(WorkoutEntryExerciseRecord exercise) => UpdateRecord(exercise);
    public void DeleteExercise(WorkoutEntryExerciseRecord exercise) => DeleteRecord(exercise);

    public WorkoutEntrySetRecord SaveSet(WorkoutEntrySetRecord set) => SaveRecord(set);
    public WorkoutEntrySetRecord UpdateSet(WorkoutEntrySetRecord set) => UpdateRecord(set);
    public void DeleteSet(WorkoutEntrySetRecord set) => DeleteRecord(set);

    public SearchEntriesDto SearchEntries(SearchEntriesParameters parameters)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<WorkoutEntryRecord>()
            .FetchMany(x => x.Exercises)
            .Where(x => x.StartAt >= parameters.StartAt && x.StartAt <= parameters.EndAt)
            .ToFuture();

        var entries = query
            .Where(x => x.StartAt >= parameters.StartAt && x.StartAt <= parameters.EndAt)
            .Take(parameters.PageSize)
            .Skip(parameters.PageSize * (parameters.PageNumber - 1))
            .OrderByDescending(x => x.StartAt)
            .ToList();

        session
            .Query<WorkoutEntryExerciseRecord>()
            .Fetch(x => x.Entry)
            .Where(x => x.Entry.StartAt >= parameters.StartAt && x.Entry.StartAt <= parameters.EndAt)
            .FetchMany(x => x.Sets)
            .ToFuture();

        var entryCount = session
            .Query<WorkoutEntryRecord>()
            .ToFutureValue(x => x.Count());

        transaction.Commit();

        return new SearchEntriesDto
        {
            Entries = entries.ToList(),
            EntryCount = entryCount.Value,
            PageSize = parameters.PageSize,
            PageNumber = parameters.PageNumber,
            PageCount = (int)Math.Ceiling((double)entryCount.Value / parameters.PageSize)
        };
    }

    public Result<WorkoutEntryRecord> GetEntryByReference(Guid reference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<WorkoutEntryRecord>()
            .FetchMany(x => x.Exercises)
            .Where(x => x.Reference == reference)
            .ToFuture();

        session
            .Query<WorkoutEntryExerciseRecord>()
            .Where(x => x.Entry.Reference == reference)
            .FetchMany(x => x.Sets)
            .ToFuture();

        var entry = query
            .SingleOrDefault();

        if (entry == null)
            return Result<WorkoutEntryRecord>.Failure($"Unable to find entry with reference: '{reference}'.");

        transaction.Commit();

        return entry;
    }

    public List<WorkoutEntryRecord> GetEntries()
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<WorkoutEntryRecord>()
            .FetchMany(x => x.Exercises)
            .ToFuture();

        session
            .Query<WorkoutEntryExerciseRecord>()
            .FetchMany(x => x.Sets)
            .ToFuture();

        var entries = query
            .OrderByDescending(x => x.StartAt)
            .ToList();

        transaction.Commit();

        return entries;
    }
}
using Data.Records;
using Data.Types;
using NetApiLibs.Type;
using NHibernate.Linq;

namespace Data.Repositories;

public interface IWorkoutDiaryRepository
{
    Result<WorkoutEntryRecord> GetEntryByReference(Guid reference);
    WorkoutEntryRecord SaveEntry(WorkoutEntryRecord entry);
    WorkoutEntryRecord UpdateEntry(WorkoutEntryRecord entry);
    List<WorkoutEntryRecord> GetEntries();
}

public sealed class WorkoutDiaryRepository : BaseRepository, IWorkoutDiaryRepository
{
    public WorkoutDiaryRepository(IApiDatabase database) : base(database)
    {
    }

    public WorkoutEntryRecord SaveEntry(WorkoutEntryRecord entry) => SaveRecord(entry);
    public WorkoutEntryRecord UpdateEntry(WorkoutEntryRecord entry) => UpdateRecord(entry);

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

        var entries = query.ToList();

        transaction.Commit();

        return entries;
    }
}
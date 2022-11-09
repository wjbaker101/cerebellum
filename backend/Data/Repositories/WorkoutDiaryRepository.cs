using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories;

public interface IWorkoutDiaryRepository
{
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

    public List<WorkoutEntryRecord> GetEntries()
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var entries = session
            .Query<WorkoutEntryRecord>()
            .FetchMany(x => x.Exercises)
            .ThenFetchMany(x => x.Sets)
            .ToList();

        transaction.Commit();

        return entries;
    }
}
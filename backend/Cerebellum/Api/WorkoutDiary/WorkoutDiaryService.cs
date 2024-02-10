using Cerebellum.Api.WorkoutDiary.Services;
using Cerebellum.Api.WorkoutDiary.Types;
using Data.Mappers;
using Data.Records;
using Data.Repositories.WorkoutDiary;
using Data.Repositories.WorkoutDiary.Types;
using DotNetLibs.Core.Types;

namespace Cerebellum.Api.WorkoutDiary;

public interface IWorkoutDiaryService
{
    Result<SearchEntriesResponse> SearchEntries(SearchEntriesRequest request);
    Result<GetEntryResponse> GetEntryByReference(Guid reference);
    Result<GetEntriesResponse> GetEntries();
    Result<CreateEntryResponse> CreateEntry(CreateEntryRequest request);
    Result<UpdateEntryResponse> UpdateEntry(Guid reference, UpdateEntryRequest request);
    Result DeleteEntry(Guid reference);
}

public sealed class WorkoutDiaryService : IWorkoutDiaryService
{
    private readonly IWorkoutDiaryRepository _workoutDiaryRepository;
    private readonly UpdateWorkoutEntryService _updateWorkoutEntryService;

    public WorkoutDiaryService(IWorkoutDiaryRepository workoutDiaryRepository)
    {
        _workoutDiaryRepository = workoutDiaryRepository;
        _updateWorkoutEntryService = new UpdateWorkoutEntryService(workoutDiaryRepository);
    }

    public Result<SearchEntriesResponse> SearchEntries(SearchEntriesRequest request)
    {
        var searchEntries = _workoutDiaryRepository.SearchEntries(new SearchEntriesParameters
        {
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber
        });

        return new SearchEntriesResponse
        {
            Entries = searchEntries.Entries.ConvertAll(WorkoutDiaryMapper.MapEntry)
        };
    }

    public Result<GetEntryResponse> GetEntryByReference(Guid reference)
    {
        var entryResult = _workoutDiaryRepository.GetEntryByReference(reference);
        if (!entryResult.TrySuccess(out var entry))
            return Result<GetEntryResponse>.FromFailure(entryResult);

        return new GetEntryResponse
        {
            Entry = WorkoutDiaryMapper.MapEntry(entry)
        };
    }

    public Result<GetEntriesResponse> GetEntries()
    {
        var entries = _workoutDiaryRepository.GetEntries();

        return new GetEntriesResponse
        {
            Entries = entries.ConvertAll(WorkoutDiaryMapper.MapEntry)
        };
    }

    public Result<CreateEntryResponse> CreateEntry(CreateEntryRequest request)
    {
        var entry = _workoutDiaryRepository.SaveEntry(new WorkoutEntryRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            Weight = request.Weight,
            Exercises = new List<WorkoutEntryExerciseRecord>()
        });

        var exercises = new List<WorkoutEntryExerciseRecord>();
        foreach (var newExercise in request.Exercises)
        {
            var exercise = _workoutDiaryRepository.SaveExercise(new WorkoutEntryExerciseRecord
            {
                Reference = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Name = newExercise.Name,
                Entry = entry,
                Sets = new List<WorkoutEntrySetRecord>()
            });

            var sets = new List<WorkoutEntrySetRecord>();
            foreach (var newSet in newExercise.Sets)
            {
                sets.Add(_workoutDiaryRepository.SaveSet(new WorkoutEntrySetRecord
                {
                    Reference = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Exercise = exercise,
                    Repetitions = newSet.Repetitions,
                    Weight = newSet.Weight
                }));
            }

            exercise.Sets = sets;
            exercises.Add(exercise);
        }
        entry.Exercises = exercises;

        return new CreateEntryResponse
        {
            Entry = WorkoutDiaryMapper.MapEntry(entry)
        };
    }

    public Result<UpdateEntryResponse> UpdateEntry(Guid reference, UpdateEntryRequest request)
    {
        return _updateWorkoutEntryService.UpdateEntry(reference, request);
    }

    public Result DeleteEntry(Guid reference)
    {
        var entryResult = _workoutDiaryRepository.GetEntryByReference(reference);
        if (!entryResult.TrySuccess(out var entry))
            return Result<UpdateEntryResponse>.FromFailure(entryResult);

        var exercises = entry.Exercises;
        var sets = exercises.SelectMany(x => x.Sets).ToList();

        foreach (var set in sets)
            _workoutDiaryRepository.DeleteSet(set);

        foreach (var exercise in exercises)
            _workoutDiaryRepository.DeleteExercise(exercise);

        _workoutDiaryRepository.DeleteEntry(entry);

        return Result.Success();
    }
}
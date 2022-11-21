using Api.WorkoutDiary.Types;
using Core.Model;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Extension;
using NetApiLibs.Type;

namespace Api.WorkoutDiary;

public interface IWorkoutDiaryService
{
    Result<GetEntryResponse> GetEntryByReference(Guid reference);
    Result<GetEntriesResponse> GetEntries();
    Result<CreateEntryResponse> CreateEntry(CreateEntryRequest request);
    Result<UpdateEntryResponse> UpdateEntry(Guid reference, UpdateEntryRequest request);
}

public sealed class WorkoutDiaryService : IWorkoutDiaryService
{
    private readonly IWorkoutDiaryRepository _workoutDiaryRepository;

    public WorkoutDiaryService(IWorkoutDiaryRepository workoutDiaryRepository)
    {
        _workoutDiaryRepository = workoutDiaryRepository;
    }

    public Result<GetEntryResponse> GetEntryByReference(Guid reference)
    {
        var entryResult = _workoutDiaryRepository.GetEntryByReference(reference);
        if (!entryResult.TrySuccess(out var entry))
            return Result<GetEntryResponse>.FromFailure(entryResult);

        return new GetEntryResponse
        {
            Entry = new WorkoutEntry
            {
                Reference = entry.Reference,
                CreatedAt = entry.CreatedAt,
                Date = entry.Date,
                StartTime = entry.StartTime,
                EndTime = entry.EndTime,
                Weight = entry.Weight,
                Exercises = entry.Exercises.ConvertAll(exercise => new WorkoutEntryExercise
                {
                    Reference = exercise.Reference,
                    CreatedAt = exercise.CreatedAt,
                    Name = exercise.Name,
                    Sets = exercise.Sets.ConvertAll(set => new WorkoutEntrySet
                    {
                        Reference = set.Reference,
                        CreatedAt = set.CreatedAt,
                        Repetitions = set.Repetitions,
                        Weight = set.Weight
                    })
                })
            }
        };
    }

    public Result<GetEntriesResponse> GetEntries()
    {
        var entries = _workoutDiaryRepository.GetEntries();

        return new GetEntriesResponse
        {
            Entries = entries.ConvertAll(entry => new WorkoutEntry
            {
                Reference = entry.Reference,
                CreatedAt = entry.CreatedAt,
                Date = entry.Date,
                StartTime = entry.StartTime,
                EndTime = entry.EndTime,
                Weight = entry.Weight,
                Exercises = entry.Exercises.ConvertAll(exercise => new WorkoutEntryExercise
                {
                    Reference = exercise.Reference,
                    CreatedAt = exercise.CreatedAt,
                    Name = exercise.Name,
                    Sets = exercise.Sets.ConvertAll(set => new WorkoutEntrySet
                    {
                        Reference = set.Reference,
                        CreatedAt = set.CreatedAt,
                        Repetitions = set.Repetitions,
                        Weight = set.Weight
                    })
                })
            })
        };
    }

    public Result<CreateEntryResponse> CreateEntry(CreateEntryRequest request)
    {
        var entry = _workoutDiaryRepository.SaveEntry(new WorkoutEntryRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Date = request.Date.Date,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Weight = request.Weight
        });

        return new CreateEntryResponse
        {
            Entry = new WorkoutEntry
            {
                Reference = entry.Reference,
                CreatedAt = entry.CreatedAt,
                Date = entry.Date,
                StartTime = entry.StartTime,
                EndTime = entry.EndTime,
                Weight = entry.Weight
            }
        };
    }

    public Result<UpdateEntryResponse> UpdateEntry(Guid reference, UpdateEntryRequest request)
    {
        var entryResult = _workoutDiaryRepository.GetEntryByReference(reference);
        if (!entryResult.TrySuccess(out var entry))
            return Result<UpdateEntryResponse>.FromFailure(entryResult);

        entry.Date = request.Date;
        entry.StartTime = request.StartTime;
        entry.EndTime = request.EndTime;
        entry.Weight = request.Weight;

        _workoutDiaryRepository.UpdateEntry(entry);

        UpdateEntryExercises(entry, request);
        UpdateEntrySets(entry, request);

        return new UpdateEntryResponse
        {
            Entry = new WorkoutEntry
            {
                Reference = entry.Reference,
                CreatedAt = entry.CreatedAt,
                Date = entry.Date,
                StartTime = entry.StartTime,
                EndTime = entry.EndTime,
                Weight = entry.Weight
            }
        };
    }

    public void UpdateEntryExercises(WorkoutEntryRecord entry, UpdateEntryRequest request)
    {
        foreach (var exercise in entry.Exercises)
        {
            var requestExercise = request.Exercises.SingleOrDefault(x => x.Reference == exercise.Reference);
            if (requestExercise == null)
            {
                _workoutDiaryRepository.DeleteExercise(exercise);
            }
            else
            {
                exercise.Name = requestExercise.Name;

                _workoutDiaryRepository.UpdateExercise(exercise);
            }
        }

        foreach (var newExercises in request.Exercises.Where(x => !x.Reference.HasValue))
        {
            _workoutDiaryRepository.SaveExercise(new WorkoutEntryExerciseRecord
            {
                Reference = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Name = newExercises.Name
            });
        }
    }

    private void UpdateEntrySets(WorkoutEntryRecord entry, UpdateEntryRequest request)
    {
        var sets = entry.Exercises.SelectMany(x => x.Sets);
        var requestSets = request.Exercises.SelectMany(x => x.Sets).ToList();

        foreach (var set in sets)
        {
            var requestSet = requestSets.SingleOrDefault(x => x.Reference == set.Reference);
            if (requestSet == null)
            {
                _workoutDiaryRepository.DeleteSet(set);
            }
            else
            {
                set.Repetitions = requestSet.Repetitions;
                set.Weight = requestSet.Weight;

                _workoutDiaryRepository.UpdateSet(set);
            }
        }

        foreach (var newSet in requestSets.Where(x => !x.Reference.HasValue))
        {
            _workoutDiaryRepository.SaveSet(new WorkoutEntrySetRecord
            {
                Reference = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Repetitions = newSet.Repetitions,
                Weight = newSet.Weight
            });
        }
    }
}
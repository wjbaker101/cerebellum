using Api.WorkoutDiary.Types;
using Core.Model;
using Data.Records;
using Data.Repositories;
using NetApiLibs.Type;

namespace Api.WorkoutDiary.Services;

public sealed class UpdateWorkoutEntryService
{
    private readonly IWorkoutDiaryRepository _workoutDiaryRepository;

    public UpdateWorkoutEntryService(IWorkoutDiaryRepository workoutDiaryRepository)
    {
        _workoutDiaryRepository = workoutDiaryRepository;
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

        UpdateExistingExercises(entry, request.Exercises.Where(x => x.Reference.HasValue).ToList());
        SaveNewExercises(entry, request.Exercises.Where(x => !x.Reference.HasValue).ToList());

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

    private void UpdateExistingExercises(WorkoutEntryRecord entry, List<UpdateEntryRequest.Exercise> updatedExercises)
    {
        foreach (var exercise in entry.Exercises)
        {
            var requestExercise = updatedExercises.SingleOrDefault(x => x.Reference == exercise.Reference);
            if (requestExercise == null)
            {
                foreach (var set in exercise.Sets)
                    _workoutDiaryRepository.DeleteSet(set);

                _workoutDiaryRepository.DeleteExercise(exercise);
            }
            else
            {
                exercise.Name = requestExercise.Name;

                _workoutDiaryRepository.UpdateExercise(exercise);

                foreach (var set in exercise.Sets)
                {
                    var requestSet = requestExercise.Sets.SingleOrDefault(x => x.Reference == set.Reference);
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

                foreach (var newSet in requestExercise.Sets.Where(x => !x.Reference.HasValue))
                {
                    _workoutDiaryRepository.SaveSet(new WorkoutEntrySetRecord
                    {
                        Reference = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow,
                        Exercise = exercise,
                        Repetitions = newSet.Repetitions,
                        Weight = newSet.Weight
                    });
                }
            }
        }
    }

    private void SaveNewExercises(WorkoutEntryRecord entry, List<UpdateEntryRequest.Exercise> newExercises)
    {
        foreach (var newExercise in newExercises)
        {
            var exercise = _workoutDiaryRepository.SaveExercise(new WorkoutEntryExerciseRecord
            {
                Reference = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Name = newExercise.Name,
                Entry = entry
            });

            foreach (var newSet in newExercise.Sets)
            {
                _workoutDiaryRepository.SaveSet(new WorkoutEntrySetRecord
                {
                    Reference = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Exercise = exercise,
                    Repetitions = newSet.Repetitions,
                    Weight = newSet.Weight
                });
            }
        }
    }
}
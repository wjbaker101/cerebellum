using Cerebellum.Api.WorkoutDiary.Types;
using Data.Mappers;
using Data.Records;
using Data.Repositories.WorkoutDiary;
using DotNetLibs.Core.Types;

namespace Cerebellum.Api.WorkoutDiary.Services;

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

        entry.StartAt = request.StartAt;
        entry.EndAt = request.EndAt;
        entry.Weight = request.Weight;

        _workoutDiaryRepository.UpdateEntry(entry);

        UpdateExistingExercises(entry, request.Exercises.Where(x => x.Reference.HasValue).ToList());
        SaveNewExercises(entry, request.Exercises.Where(x => !x.Reference.HasValue).ToList());

        var updatedEntryResult = _workoutDiaryRepository.GetEntryByReference(reference);
        if (!updatedEntryResult.TrySuccess(out var updatedEntry))
            return Result<UpdateEntryResponse>.FromFailure(updatedEntryResult);

        return new UpdateEntryResponse
        {
            Entry = WorkoutDiaryMapper.MapEntry(updatedEntry)
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
                Entry = entry,
                Sets = new List<WorkoutEntrySetRecord>()
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
﻿using Api.WorkoutDiary.Services;
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
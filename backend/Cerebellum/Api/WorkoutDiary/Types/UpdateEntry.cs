﻿using Core.Model;

namespace Cerebellum.Api.WorkoutDiary.Types;

public sealed class UpdateEntryRequest
{
    public required DateTime StartAt { get; init; }
    public required DateTime? EndAt { get; init; }
    public required decimal? Weight { get; init; }
    public required List<Exercise> Exercises { get; init; }

    public sealed class Exercise
    {
        public required Guid? Reference { get; init; }
        public required string Name { get; init; }
        public required List<Set> Sets { get; init; }
    }

    public sealed class Set
    {
        public required Guid? Reference { get; init; }
        public required int Repetitions { get; init; }
        public required decimal Weight { get; init; }
    }
}

public sealed class UpdateEntryResponse
{
    public required WorkoutEntry Entry { get; init; }
}
import { Dayjs } from 'dayjs';

export interface IWorkoutEntry {
    reference: string;
    createdAt: Dayjs;
    startAt: Dayjs;
    endAt: Dayjs | null;
    weight: number | null;
    exercises: Array<IWorkoutExercise>;
}

export interface IWorkoutExercise {
    reference: string;
    createdAt: Dayjs;
    name: string;
    sets: Array<IWorkoutExerciseSet>;
}

export interface IWorkoutExerciseSet {
    reference: string;
    createdAt: Dayjs;
    repetitions: number;
    weight: number;
}
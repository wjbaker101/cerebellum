import { Dayjs } from 'dayjs';

export interface IWorkoutEntry {
    date: Dayjs;
    startTime: Dayjs;
    endTime: Dayjs | null;
    weight: number | null;
    exercises: Array<IWorkoutEntry>;
}

export interface IWorkoutExercise {
    name: string;
    sets: Array<IWorkoutExerciseSet>;
}

export interface IWorkoutExerciseSet {
    repetitions: number;
    weight: number;
}
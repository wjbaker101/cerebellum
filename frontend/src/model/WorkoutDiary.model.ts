import { Dayjs } from 'dayjs';

export interface IWorkoutDiaryEntry {
    reference: string;
    createdAt: Dayjs;
    date: Dayjs;
    startTime: Dayjs;
    endTime: Dayjs | null;
    weight: number | null;
    exercises: Array<IWorkoutDiaryExercise>;
}

export interface IWorkoutDiaryExercise {
    reference: string;
    createdAt: Dayjs;
    name: string;
    sets: Array<IWorkoutDiarySet>;
}

export interface IWorkoutDiarySet {
    reference: string;
    createdAt: Dayjs;
    repetitions: number;
    weight: number;
}
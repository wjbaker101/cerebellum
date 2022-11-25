import { IApiWorkoutDiaryEntry } from './ApiWorkoutDiary.type';

export interface ICreateWorkoutDiaryEntryRequest {
    date: string;
    startTime: string;
    endTime: string | null;
    weight: number | null;
    exercises: Array<{
        reference: string | null;
        name: string;
        sets: Array<{
            reference: string | null;
            repetitions: number;
            weight: number;
        }>;
    }>;
}

export interface ICreateWorkoutDiaryEntryResponse {
    entry: IApiWorkoutDiaryEntry;
}
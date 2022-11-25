import { IApiWorkoutDiaryEntry } from './ApiWorkoutDiary.type';

export interface IUpdateWorkoutDiaryEntryRequest {
    startAt: string;
    endAt: string | null;
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

export interface IUpdateWorkoutDiaryEntryResponse {
    entry: IApiWorkoutDiaryEntry;
}
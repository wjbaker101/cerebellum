import { IApiWorkoutDiaryEntry } from './ApiWorkoutDiary.type';

export interface ISearchWorkoutDiaryEntriesResponse {
    entries: Array<IApiWorkoutDiaryEntry>;
}
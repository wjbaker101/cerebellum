export interface IApiWorkoutDiaryEntry {
    reference: string;
    createdAt: string;
    startAt: string;
    endAt: string | null;
    weight: number | null;
    exercises: Array<IApiWorkoutDiaryExercise>;
}

export interface IApiWorkoutDiaryExercise {
    reference: string;
    createdAt: string;
    name: string;
    sets: Array<IApiWorkoutDiarySet>;
}

export interface IApiWorkoutDiarySet {
    reference: string;
    createdAt: string;
    repetitions: number;
    weight: number;
}
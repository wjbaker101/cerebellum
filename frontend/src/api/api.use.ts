import dayjs from 'dayjs';

import { apiClient } from '@/api/client';

import { IWorkoutEntry, IWorkoutExercise, IWorkoutExerciseSet } from '@/view/workout-diary/model/WorkoutEntry.model';
import { ISearchWorkoutDiaryEntriesResponse } from '@/api/type/SearchWorkoutDiaryEntries.type';
import { IGetWorkoutDiaryEntryByReferenceResponse } from '@/api/type/GetWorkoutDiaryEntryByReference.type';
import { ICreateWorkoutDiaryEntryRequest, ICreateWorkoutDiaryEntryResponse } from '@/api/type/CreateWorkoutDiaryEntry.type';
import { IUpdateWorkoutDiaryEntryRequest, IUpdateWorkoutDiaryEntryResponse } from '@/api/type/UpdateWorkoutDiaryEntry.type';
import { ILogInRequest, ILogInResponse } from '@/api/type/LogIn.type';

export const useApi = function () {
    return {

        auth: {

            async logIn(request: ILogInRequest): Promise<ILogInResponse | Error> {
                const response = await apiClient.post<ILogInResponse>({
                    body: request,
                    url: '/auth/log-in',
                    auth: {
                        required: false,
                        use: false,
                    },
                });

                if (response instanceof Error)
                    return response;

                return response;
            },

        },

        workoutDiary: {
            async search(): Promise<Array<IWorkoutEntry> | Error> {
                const response = await apiClient.get<ISearchWorkoutDiaryEntriesResponse>({
                    url: '/workout-diary/entries',
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const entries = response.entries;

                return entries.map(entry => ({
                    reference: entry.reference,
                    createdAt: dayjs(entry.createdAt),
                    startAt: dayjs(entry.startAt),
                    endAt: entry.endAt !== null ? dayjs(entry.endAt) : null,
                    weight: entry.weight,
                    exercises: entry.exercises.map(exercise => (<IWorkoutExercise>{
                        reference: exercise.reference,
                        createdAt: dayjs(exercise.createdAt),
                        name: exercise.name,
                        sets: exercise.sets.map(set => (<IWorkoutExerciseSet>{
                            reference: set.reference,
                            createdAt: dayjs(set.createdAt),
                            repetitions: set.repetitions,
                            weight: set.weight,
                        })),
                    })),
                }));
            },

            async getEntryByReference(reference: string): Promise<IWorkoutEntry | Error> {
                const response = await apiClient.get<IGetWorkoutDiaryEntryByReferenceResponse>({
                    url: `/workout-diary/entry/${reference}`,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const entry = response.entry;

                return {
                    reference: entry.reference,
                    createdAt: dayjs(entry.createdAt),
                    startAt: dayjs(entry.startAt),
                    endAt: entry.endAt !== null ? dayjs(entry.endAt) : null,
                    weight: entry.weight,
                    exercises: entry.exercises.map(exercise => (<IWorkoutExercise>{
                        reference: exercise.reference,
                        createdAt: dayjs(exercise.createdAt),
                        name: exercise.name,
                        sets: exercise.sets.map(set => (<IWorkoutExerciseSet>{
                            reference: set.reference,
                            createdAt: dayjs(set.createdAt),
                            repetitions: set.repetitions,
                            weight: set.weight,
                        })),
                    })),
                };
            },

            async createEntry(request: ICreateWorkoutDiaryEntryRequest): Promise<IWorkoutEntry | Error> {
                const response = await apiClient.post<ICreateWorkoutDiaryEntryResponse>({
                    url: '/workout-diary/entry',
                    body: {
                        startAt: request.startAt,
                        endAt: request.endAt,
                        weight: request.weight,
                        exercises: request.exercises.map(exercise => ({
                            reference: exercise.reference,
                            name: exercise.name,
                            sets: exercise.sets.map(set => ({
                                reference: set.reference,
                                repetitions: set.repetitions,
                                weight: set.weight,
                            })),
                        })),
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const entry = response.entry;

                return {
                    reference: entry.reference,
                    createdAt: dayjs(entry.createdAt),
                    startAt: dayjs(entry.startAt),
                    endAt: entry.endAt !== null ? dayjs(entry.endAt) : null,
                    weight: entry.weight,
                    exercises: entry.exercises.map(exercise => (<IWorkoutExercise>{
                        reference: exercise.reference,
                        createdAt: dayjs(exercise.createdAt),
                        name: exercise.name,
                        sets: exercise.sets.map(set => (<IWorkoutExerciseSet>{
                            reference: set.reference,
                            createdAt: dayjs(set.createdAt),
                            repetitions: set.repetitions,
                            weight: set.weight,
                        })),
                    })),
                };
            },

            async updateEntry(reference: string, request: IUpdateWorkoutDiaryEntryRequest): Promise<IWorkoutEntry | Error> {
                const response = await apiClient.put<IUpdateWorkoutDiaryEntryResponse>({
                    url: `/workout-diary/entry/${reference}`,
                    body: {
                        startAt: request.startAt,
                        endAt: request.endAt,
                        weight: request.weight,
                        exercises: request.exercises.map(exercise => ({
                            reference: exercise.reference,
                            name: exercise.name,
                            sets: exercise.sets.map(set => ({
                                reference: set.reference,
                                repetitions: set.repetitions,
                                weight: set.weight,
                            })),
                        })),
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const entry = response.entry;

                return {
                    reference: entry.reference,
                    createdAt: dayjs(entry.createdAt),
                    startAt: dayjs(entry.startAt),
                    endAt: entry.endAt !== null ? dayjs(entry.endAt) : null,
                    weight: entry.weight,
                    exercises: entry.exercises.map(exercise => ({
                        reference: exercise.reference,
                        createdAt: dayjs(exercise.createdAt),
                        name: exercise.name,
                        sets: exercise.sets.map(set => ({
                            reference: set.reference,
                            createdAt: dayjs(set.createdAt),
                            repetitions: set.repetitions,
                            weight: set.weight,
                        })),
                    })),
                };
            },

            async deleteEntry(reference: string): Promise<void | Error> {
                const response = await apiClient.delete({
                    url: `/workout-diary/entry/${reference}`,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;
            },
        },

    };
};
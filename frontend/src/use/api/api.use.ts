import dayjs, { Dayjs } from 'dayjs';

import { CalendarRecurringPeriod, ICalendarEntry } from '@/model/CalendarEntry.model';
import { INote } from '@/model/Note.model';
import { IListum, IListumItem } from '@/model/Listum.model';

import { IApiResponse } from '@/use/api/type/ApiResponse.type';

import { ISearchCalendarEntriesResponse } from '@/use/api/type/SearchCalendarEntries.type';
import { IAddCalendarEntryResponse } from '@/use/api/type/AddCalendarEntry.type';
import { ISearchNotesResponse } from '@/use/api/type/SearchNotes.type';
import { IGetNoteResponse } from '@/use/api/type/GetNote.type';
import { ICreateNoteResponse } from '@/use/api/type/CreateNote.type';
import { IUpdateNoteResponse } from '@/use/api/type/UpdateNote.type';
import { IGetListByReference } from '@/use/api/type/GetListByReference.type';
import { GetListsResponse } from '@/use/api/type/GetLists.type';
import { ICreateListResponse } from '@/use/api/type/CreateList.type';
import { IUpdateListResponse } from '@/use/api/type/UpdateList.type';
import { ICreateListItemResponse } from '@/use/api/type/CreateListItem.type';
import { IUpdateListItemResponse } from '@/use/api/type/UpdateListItem.type';
import { IWorkoutDiaryEntry, IWorkoutDiaryExercise, IWorkoutDiarySet } from '@/model/WorkoutDiary.model';
import { ISearchWorkoutDiaryEntriesResponse } from './type/SearchWorkoutDiaryEntries.type';
import { IGetWorkoutDiaryEntryByReferenceResponse } from './type/GetWorkoutDiaryEntryByReference.type';
import { ICreateWorkoutDiaryEntryResponse } from './type/CreateWorkoutDiaryEntry.type';
import { IUpdateWorkoutDiaryEntryResponse } from './type/UpdateWorkoutDiaryEntry.type';

const baseUrl = '/api';

export const useApi = function () {
    return {

        calendar: {
            async searchEntries(startAt: Dayjs, endAt: Dayjs): Promise<Array<ICalendarEntry>> {
                const params = new URLSearchParams({
                    startAt: startAt.toISOString(),
                    endAt: endAt.toISOString()
                });

                const response = await fetch(`${baseUrl}/calendar/entries?${params.toString()}`);
                const json = await response.json() as IApiResponse<ISearchCalendarEntriesResponse>;

                const entries = json.result.entries;

                return entries.map(x => ({
                    reference: x.reference,
                    createdAt: dayjs(x.createdAt),
                    description: x.description,
                    startAt: dayjs(x.startAt),
                    endAt: dayjs(x.endAt),
                    recurringPeriod: mapRecurringPeriod(x.recurringPeriod),
                }));
            },

            async addEntry(request: {
                description: string,
                startAt: Dayjs,
                endAt: Dayjs,
                recurringPeriod: CalendarRecurringPeriod,
            }): Promise<ICalendarEntry> {

                const response = await fetch(`${baseUrl}/calendar/entry`, {
                    method: 'post',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        description: request.description,
                        startAt: request.startAt.toISOString(),
                        endAt: request.endAt.toISOString(),
                        recurringPeriod: mapApiRecurringPeriod(request.recurringPeriod),
                    }),
                });
                const json = await response.json() as IApiResponse<IAddCalendarEntryResponse>;

                const entry = json.result.calendarEntry;

                return {
                    reference: entry.reference,
                    createdAt: dayjs(entry.createdAt),
                    description: entry.description,
                    startAt: dayjs(entry.startAt),
                    endAt: dayjs(entry.endAt),
                    recurringPeriod: mapRecurringPeriod(entry.recurringPeriod),
                };
            },
        },

        notes: {
            async search(): Promise<Array<INote>> {
                const response = await fetch(`${baseUrl}/notes`);
                const json = await response.json() as IApiResponse<ISearchNotesResponse>;

                const notes = json.result.notes;

                return notes.map(x => ({
                    reference: x.reference,
                    createdAt: dayjs(x.createdAt),
                    title: x.title,
                    content: x.content,
                }));
            },

            async getNote(reference: string): Promise<INote> {
                const response = await fetch(`${baseUrl}/note/${reference}`);
                const json = await response.json() as IApiResponse<IGetNoteResponse>;

                const note = json.result.note;

                return {
                    reference: note.reference,
                    createdAt: dayjs(note.createdAt),
                    title: note.title,
                    content: note.content,
                };
            },

            async createNote(request: {
                title: string;
                content: string;
            }): Promise<INote> {
                const response = await fetch(`${baseUrl}/note`, {
                    method: 'post',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        title: request.title,
                        content: request.content,
                    }),
                });
                const json = await response.json() as IApiResponse<ICreateNoteResponse>;

                const note = json.result.note;

                return {
                    reference: note.reference,
                    createdAt: dayjs(note.createdAt),
                    title: note.title,
                    content: note.content,
                };
            },

            async updateNote(reference: string, request: {
                title: string;
                content: string;
            }): Promise<INote> {
                const response = await fetch(`${baseUrl}/note/${reference}`, {
                    method: 'put',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        title: request.title,
                        content: request.content,
                    }),
                });
                const json = await response.json() as IApiResponse<IUpdateNoteResponse>;

                const note = json.result.note;

                return {
                    reference: note.reference,
                    createdAt: dayjs(note.createdAt),
                    title: note.title,
                    content: note.content,
                };
            },

            async deleteNote(reference: string): Promise<void> {
                await fetch(`${baseUrl}/note/${reference}`, {
                    method: 'delete',
                });
            },
        },

        listum: {
            async search(): Promise<Array<IListum>> {
                const response = await fetch(`${baseUrl}/listum/lists`);

                const json = await response.json() as IApiResponse<GetListsResponse>;

                const lists = json.result.lists;

                return lists.map(list => ({
                    reference: list.reference,
                    createdAt: dayjs(list.createdAt),
                    title: list.title,
                    items: list.items.map(item => ({
                        reference: item.reference,
                        createdAt: dayjs(item.createdAt),
                        content: item.content,
                        listOrder: item.listOrder,
                    })),
                }));
            },

            async getListByReference(reference: string): Promise<IListum> {
                const response = await fetch(`${baseUrl}/listum/list/${reference}`);

                const json = await response.json() as IApiResponse<IGetListByReference>;

                const list = json.result.list;

                return {
                    reference: list.reference,
                    createdAt: dayjs(list.createdAt),
                    title: list.title,
                    items: list.items.map(x => ({
                        reference: x.reference,
                        createdAt: dayjs(x.createdAt),
                        content: x.content,
                        listOrder: x.listOrder,
                    })),
                };
            },

            async createList(request: {
                title: string;
            }): Promise<IListum> {
                const response = await fetch(`${baseUrl}/listum/list`, {
                    method: 'post',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        title: request.title,
                    }),
                });
                const json = await response.json() as IApiResponse<ICreateListResponse>;

                const list = json.result.list;

                return {
                    reference: list.reference,
                    createdAt: dayjs(list.createdAt),
                    title: list.title,
                    items: [],
                };
            },

            async updateList(reference: string, request: {
                title: string;
            }): Promise<IListum> {
                const response = await fetch(`${baseUrl}/listum/list/${reference}`, {
                    method: 'put',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        title: request.title,
                    }),
                });
                const json = await response.json() as IApiResponse<IUpdateListResponse>;

                const list = json.result.list;

                return {
                    reference: list.reference,
                    createdAt: dayjs(list.createdAt),
                    title: list.title,
                    items: list.items.map(x => ({
                        reference: x.reference,
                        createdAt: dayjs(x.createdAt),
                        content: x.content,
                        listOrder: x.listOrder,
                    })),
                };
            },

            async reorderList(reference: string, items: Array<IListumItem>): Promise<void> {
                const order: Record<string, number> = {};
                for (let index = 0; index < items.length; ++index) {
                    order[items[index].reference] = index;
                }

                await fetch(`${baseUrl}/listum/list/${reference}/reorder`, {
                    method: 'post',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        order,
                    }),
                });
            },

            async addListItem(listReference: string, request: {
                content: string;
            }): Promise<IListumItem> {
                const response = await fetch(`${baseUrl}/listum/list/${listReference}/item`, {
                    method: 'post',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        content: request.content,
                    }),
                });
                const json = await response.json() as IApiResponse<ICreateListItemResponse>;

                const listItem = json.result.listItem;

                return {
                    reference: listItem.reference,
                    createdAt: dayjs(listItem.createdAt),
                    content: listItem.content,
                    listOrder: listItem.listOrder,
                };
            },

            async updateListItem(listReference: string, itemReference: string, request: {
                content: string;
            }): Promise<IListumItem> {
                const response = await fetch(`${baseUrl}/listum/list/${listReference}/item/${itemReference}`, {
                    method: 'put',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        content: request.content,
                    }),
                });
                const json = await response.json() as IApiResponse<IUpdateListItemResponse>;

                const listItem = json.result.listItem;

                return {
                    reference: listItem.reference,
                    createdAt: dayjs(listItem.createdAt),
                    content: listItem.content,
                    listOrder: listItem.listOrder,
                };
            },
        },

        workoutDiary: {
            async search(): Promise<Array<IWorkoutDiaryEntry>> {
                const response = await fetch(`${baseUrl}/workout-diary/entries`);

                const json = await response.json() as IApiResponse<ISearchWorkoutDiaryEntriesResponse>;

                const entries = json.result.entries;

                return entries.map(entry => ({
                    reference: entry.reference,
                    createdAt: dayjs(entry.createdAt),
                    date: dayjs(entry.date),
                    startTime: dayjs(entry.startTime),
                    endTime: entry.endTime !== null ? dayjs(entry.endTime) : null,
                    weight: entry.weight,
                    exercises: entry.exercises.map(exercise => (<IWorkoutDiaryExercise>{
                        reference: exercise.reference,
                        createdAt: dayjs(exercise.createdAt),
                        name: exercise.name,
                        sets: exercise.sets.map(set => (<IWorkoutDiarySet>{
                            reference: set.reference,
                            createdAt: dayjs(set.createdAt),
                            repetitions: set.repetitions,
                            weight: set.weight,
                        })),
                    })),
                }));
            },

            async getEntryByReference(reference: string): Promise<IWorkoutDiaryEntry> {
                const response = await fetch(`${baseUrl}/workout-diary/entry/${reference}`);

                const json = await response.json() as IApiResponse<IGetWorkoutDiaryEntryByReferenceResponse>;

                const entry = json.result.entry;

                return {
                    reference: entry.reference,
                    createdAt: dayjs(entry.createdAt),
                    date: dayjs(entry.date),
                    startTime: dayjs(entry.startTime),
                    endTime: entry.endTime !== null ? dayjs(entry.endTime) : null,
                    weight: entry.weight,
                    exercises: entry.exercises.map(exercise => (<IWorkoutDiaryExercise>{
                        reference: exercise.reference,
                        createdAt: dayjs(exercise.createdAt),
                        name: exercise.name,
                        sets: exercise.sets.map(set => (<IWorkoutDiarySet>{
                            reference: set.reference,
                            createdAt: dayjs(set.createdAt),
                            repetitions: set.repetitions,
                            weight: set.weight,
                        })),
                    })),
                };
            },

            async createEntry(request: {
                date: string;
                startTime: string;
                endTime: string | null;
                weight: number | null;
            }): Promise<IWorkoutDiaryEntry> {
                const response = await fetch(`${baseUrl}/workout-diary/entry`, {
                    method: 'post',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        date: request.date,
                        startTime: request.startTime,
                        endTime: request.endTime,
                        weight: request.weight,
                    }),
                });
                const json = await response.json() as IApiResponse<ICreateWorkoutDiaryEntryResponse>;

                const entry = json.result.entry;

                return {
                    reference: entry.reference,
                    createdAt: dayjs(entry.createdAt),
                    date: dayjs(entry.date),
                    startTime: dayjs(entry.startTime),
                    endTime: entry.endTime !== null ? dayjs(entry.endTime) : null,
                    weight: entry.weight,
                    exercises: entry.exercises.map(exercise => (<IWorkoutDiaryExercise>{
                        reference: exercise.reference,
                        createdAt: dayjs(exercise.createdAt),
                        name: exercise.name,
                        sets: exercise.sets.map(set => (<IWorkoutDiarySet>{
                            reference: set.reference,
                            createdAt: dayjs(set.createdAt),
                            repetitions: set.repetitions,
                            weight: set.weight,
                        })),
                    })),
                };
            },

            async updateEntry(reference: string, request: {
                date: string;
                startTime: string;
                endTime: string | null;
                weight: number | null;
            }): Promise<IWorkoutDiaryEntry> {
                const response = await fetch(`${baseUrl}/workout-diary/entry/${reference}`, {
                    method: 'put',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
                        date: request.date,
                        startTime: request.startTime,
                        endTime: request.endTime,
                        weight: request.weight,
                    }),
                });
                const json = await response.json() as IApiResponse<IUpdateWorkoutDiaryEntryResponse>;

                const entry = json.result.entry;

                return {
                    reference: entry.reference,
                    createdAt: dayjs(entry.createdAt),
                    date: dayjs(entry.date),
                    startTime: dayjs(entry.startTime),
                    endTime: entry.endTime !== null ? dayjs(entry.endTime) : null,
                    weight: entry.weight,
                    exercises: entry.exercises.map(exercise => (<IWorkoutDiaryExercise>{
                        reference: exercise.reference,
                        createdAt: dayjs(exercise.createdAt),
                        name: exercise.name,
                        sets: exercise.sets.map(set => (<IWorkoutDiarySet>{
                            reference: set.reference,
                            createdAt: dayjs(set.createdAt),
                            repetitions: set.repetitions,
                            weight: set.weight,
                        })),
                    })),
                };
            },
        },

    };
};

const mapRecurringPeriod = function (value: number): CalendarRecurringPeriod {
    switch (value) {
        case 1: return 'none';
        case 2: return 'weekly';
        case 3: return 'monthly';
        case 4: return 'yearly';

        default: throw new Error(`Invalid recurring period retrieved: ${value}.`);
    }
};

const mapApiRecurringPeriod = function (value: CalendarRecurringPeriod): number {
    switch (value) {
        case 'none': return 1;
        case 'weekly': return 2;
        case 'monthly': return 3;
        case 'yearly': return 4;
    }
};
import dayjs, { Dayjs } from 'dayjs';

import { CalendarRecurringPeriod, ICalendarEntry } from '@/model/CalendarEntry.model';
import { INote } from '@/model/Note.model';
import { IList, IListItem } from '@/view/lists/model/List.model';

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
import { ISearchWorkoutDiaryEntriesResponse } from './type/SearchWorkoutDiaryEntries.type';
import { IGetWorkoutDiaryEntryByReferenceResponse } from './type/GetWorkoutDiaryEntryByReference.type';
import { ICreateWorkoutDiaryEntryRequest, ICreateWorkoutDiaryEntryResponse } from './type/CreateWorkoutDiaryEntry.type';
import { IUpdateWorkoutDiaryEntryRequest, IUpdateWorkoutDiaryEntryResponse } from './type/UpdateWorkoutDiaryEntry.type';
import { IWorkoutEntry, IWorkoutExercise, IWorkoutExerciseSet } from '@/view/workout-diary/model/WorkoutEntry.model';
import { IKanbanBoard, IKanbanColumn, IKanbanItem } from '@/view/kanban/model/KanbanBoard.model';
import { IGetKanbanBoardResponse } from './type/GetKanbanBoard.type';
import { IAddKanbanColumnRequest, IAddKanbanColumnResponse } from './type/AddKanbanColumn.type';
import { ISearchKanbanBoardsResponse } from './type/SearchKanbanBoards.type';
import { IAddKanbanItemRequest, IAddKanbanItemResponse } from './type/AddKanbanItem.type';
import { IUpdateKanbanItemRequest, IUpdateKanbanItemResponse } from './type/UpdateKanbanItem.type';
import { IUpdateKanbanBoardPositionsRequest, IUpdateKanbanBoardPositionsResponse } from './type/UpdateKanbanBoardPositions.type';
import { IUpdateKanbanColumnRequest, IUpdateKanbanColumnResponse } from './type/UpdateKanbanColumn.type';
import { IUpdateKanbanBoardRequest, IUpdateKanbanBoardResponse } from './type/UpdateKanbanBoard.type';
import { IDashboard } from '@/model/Dashboard.model';
import { IGetDashboardResponse } from './type/GetDashboard.type';
import { dashboardItemTypeMapper } from './mapper/DashboardItemType.mapper';

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

        dashboard: {
            async get(): Promise<IDashboard> {
                const response = await fetch(`${baseUrl}/dashboard`);
                const json  = await response.json() as IApiResponse<IGetDashboardResponse>;

                const dashboard = json.result;

                return {
                    items: dashboard.items.map(x => ({
                        reference: x.reference,
                        title: x.title,
                        type: dashboardItemTypeMapper.map(x.type),
                        createdAt: dayjs(x.createdAt),
                    })),
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

        lists: {
            async search(): Promise<Array<IList>> {
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

            async getListByReference(reference: string): Promise<IList> {
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
            }): Promise<IList> {
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
            }): Promise<IList> {
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

            async reorderList(reference: string, items: Array<IListItem>): Promise<void> {
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
            }): Promise<IListItem> {
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
            }): Promise<IListItem> {
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
            async search(): Promise<Array<IWorkoutEntry>> {
                const response = await fetch(`${baseUrl}/workout-diary/entries`);

                const json = await response.json() as IApiResponse<ISearchWorkoutDiaryEntriesResponse>;

                const entries = json.result.entries;

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

            async getEntryByReference(reference: string): Promise<IWorkoutEntry> {
                const response = await fetch(`${baseUrl}/workout-diary/entry/${reference}`);

                const json = await response.json() as IApiResponse<IGetWorkoutDiaryEntryByReferenceResponse>;

                const entry = json.result.entry;

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

            async createEntry(request: ICreateWorkoutDiaryEntryRequest): Promise<IWorkoutEntry> {
                const response = await fetch(`${baseUrl}/workout-diary/entry`, {
                    method: 'post',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
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
                    }),
                });
                const json = await response.json() as IApiResponse<ICreateWorkoutDiaryEntryResponse>;

                const entry = json.result.entry;

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

            async updateEntry(reference: string, request: IUpdateWorkoutDiaryEntryRequest): Promise<IWorkoutEntry> {
                const response = await fetch(`${baseUrl}/workout-diary/entry/${reference}`, {
                    method: 'put',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify({
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
                    }),
                });
                const json = await response.json() as IApiResponse<IUpdateWorkoutDiaryEntryResponse>;

                const entry = json.result.entry;

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

            async deleteEntry(reference: string): Promise<void> {
                await fetch(`${baseUrl}/workout-diary/entry/${reference}`, { method: 'delete' });
            },
        },

        kanban: {
            async search(): Promise<Array<IKanbanBoard>> {
                const response = await fetch(`${baseUrl}/kanban/search`);

                const json = await response.json() as IApiResponse<ISearchKanbanBoardsResponse>;

                const kanbanBoards = json.result.kanbanBoards;

                return kanbanBoards.map(x => ({
                    reference: x.reference,
                    createdAt: dayjs(x.createdAt),
                    title: x.title,
                    columns: [],
                }));
            },

            async getBoard(reference: string): Promise<IKanbanBoard> {
                const response = await fetch(`${baseUrl}/kanban/board/${reference}`);

                const json = await response.json() as IApiResponse<IGetKanbanBoardResponse>;

                const kanbanBoard = json.result.kanbanBoard;

                return {
                    reference: kanbanBoard.reference,
                    createdAt: dayjs(kanbanBoard.createdAt),
                    title: kanbanBoard.title,
                    columns: kanbanBoard.columns.map(column => ({
                        reference: column.reference,
                        createdAt: dayjs(column.createdAt),
                        title: column.title,
                        position: column.position,
                        items: column.items.map(item => ({
                            reference: item.reference,
                            createdAt: dayjs(item.createdAt),
                            content: item.content,
                            position: item.position,
                        })),
                    })),
                };
            },

            async updateBoard(reference: string, request: IUpdateKanbanBoardRequest): Promise<IKanbanBoard> {
                const response = await fetch(`${baseUrl}/kanban/board/${reference}`, {
                    method: 'put',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify(request),
                });

                const json = await response.json() as IApiResponse<IUpdateKanbanBoardResponse>;

                const kanbanBoard = json.result.kanbanBoard;

                return {
                    reference: kanbanBoard.reference,
                    createdAt: dayjs(kanbanBoard.createdAt),
                    title: kanbanBoard.title,
                    columns: kanbanBoard.columns.map(column => ({
                        reference: column.reference,
                        createdAt: dayjs(column.createdAt),
                        title: column.title,
                        position: column.position,
                        items: column.items.map(item => ({
                            reference: item.reference,
                            createdAt: dayjs(item.createdAt),
                            content: item.content,
                            position: item.position,
                        })),
                    })),
                };
            },

            async updateBoardPositions(boardReference: string, request: IUpdateKanbanBoardPositionsRequest): Promise<void> {
                const response = await fetch(`${baseUrl}/kanban/board/${boardReference}/positions`, {
                    method: 'put',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify(request),
                });

                await response.json() as IApiResponse<IUpdateKanbanBoardPositionsResponse>;
            },

            async addColumn(reference: string, request: IAddKanbanColumnRequest): Promise<IKanbanColumn> {
                const response = await fetch(`${baseUrl}/kanban/board/${reference}/column`, {
                    method: 'post',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify(request),
                });
                const json = await response.json() as IApiResponse<IAddKanbanColumnResponse>;

                const kanbanColumn = json.result.kanbanColumn;

                return {
                    reference: kanbanColumn.reference,
                    createdAt: dayjs(kanbanColumn.createdAt),
                    title: kanbanColumn.title,
                    position: kanbanColumn.position,
                    items: kanbanColumn.items.map(item => ({
                        reference: item.reference,
                        createdAt: dayjs(item.createdAt),
                        content: item.content,
                        position: item.position,
                    })),
                };
            },

            async updateColumn(boardReference: string, columnReference: string, request: IUpdateKanbanColumnRequest): Promise<IKanbanColumn> {
                const response = await fetch(`${baseUrl}/kanban/board/${boardReference}/column/${columnReference}`, {
                    method: 'put',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify(request),
                });
                const json = await response.json() as IApiResponse<IUpdateKanbanColumnResponse>;

                const kanbanColumn = json.result.kanbanColumn;

                return {
                    reference: kanbanColumn.reference,
                    createdAt: dayjs(kanbanColumn.createdAt),
                    title: kanbanColumn.title,
                    position: kanbanColumn.position,
                    items: kanbanColumn.items.map(item => ({
                        reference: item.reference,
                        createdAt: dayjs(item.createdAt),
                        content: item.content,
                        position: item.position,
                    })),
                };
            },

            async deleteColumn(boardReference: string, columnReference: string): Promise<void> {
                const response = await fetch(`${baseUrl}/kanban/board/${boardReference}/column/${columnReference}`, {
                    method: 'delete',
                });

                await response.json() as IApiResponse<void>;
            },

            async addItem(boardReference: string, columnReference: string, request: IAddKanbanItemRequest): Promise<IKanbanItem> {
                const response = await fetch(`${baseUrl}/kanban/board/${boardReference}/column/${columnReference}/item`, {
                    method: 'post',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify(request),
                });
                const json = await response.json() as IApiResponse<IAddKanbanItemResponse>;

                const kanbanItem = json.result.kanbanItem;

                return {
                    reference: kanbanItem.reference,
                    createdAt: dayjs(kanbanItem.createdAt),
                    content: kanbanItem.content,
                    position: kanbanItem.position,
                };
            },

            async updateItem(boardReference: string, columnReference: string, itemReference: string, request: IUpdateKanbanItemRequest): Promise<IKanbanItem> {
                const response = await fetch(`${baseUrl}/kanban/board/${boardReference}/column/${columnReference}/item/${itemReference}`, {
                    method: 'put',
                    headers: new Headers({
                        'Content-Type': 'application/json',
                    }),
                    body: JSON.stringify(request),
                });
                const json = await response.json() as IApiResponse<IUpdateKanbanItemResponse>;

                const kanbanItem = json.result.kanbanItem;

                return {
                    reference: kanbanItem.reference,
                    createdAt: dayjs(kanbanItem.createdAt),
                    content: kanbanItem.content,
                    position: kanbanItem.position,
                };
            },

            async deleteItem(boardReference: string, columnReference: string, itemReference: string): Promise<void> {
                const response = await fetch(`${baseUrl}/kanban/board/${boardReference}/column/${columnReference}/item/${itemReference}`, {
                    method: 'delete',
                });

                await response.json() as IApiResponse<void>;
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
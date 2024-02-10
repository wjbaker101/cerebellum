import dayjs, { Dayjs } from 'dayjs';

import { apiClient } from '@/api/client';

import { dashboardItemTypeMapper } from './mapper/DashboardItemType.mapper';

import { CalendarRecurringPeriod, ICalendarEntry } from '@/models/CalendarEntry.model';
import { INote } from '@/models/Note.model';
import { IList, IListItem } from '@/view/lists/model/List.model';
import { IDashboard } from '@/models/Dashboard.model';
import { IWorkoutEntry, IWorkoutExercise, IWorkoutExerciseSet } from '@/view/workout-diary/model/WorkoutEntry.model';
import { IKanbanBoard, IKanbanColumn, IKanbanItem } from '@/view/kanban/model/KanbanBoard.model';

import { ISearchCalendarEntriesResponse } from '@/api/type/SearchCalendarEntries.type';
import { IAddCalendarEntryResponse } from '@/api/type/AddCalendarEntry.type';
import { ISearchNotesResponse } from '@/api/type/SearchNotes.type';
import { IGetNoteResponse } from '@/api/type/GetNote.type';
import { ICreateNoteResponse } from '@/api/type/CreateNote.type';
import { IUpdateNoteResponse } from '@/api/type/UpdateNote.type';
import { IGetListByReference } from '@/api/type/GetListByReference.type';
import { GetListsResponse } from '@/api/type/GetLists.type';
import { ICreateListResponse } from '@/api/type/CreateList.type';
import { IUpdateListResponse } from '@/api/type/UpdateList.type';
import { ICreateListItemResponse } from '@/api/type/CreateListItem.type';
import { IUpdateListItemResponse } from '@/api/type/UpdateListItem.type';
import { ISearchWorkoutDiaryEntriesResponse } from '@/api/type/SearchWorkoutDiaryEntries.type';
import { IGetWorkoutDiaryEntryByReferenceResponse } from '@/api/type/GetWorkoutDiaryEntryByReference.type';
import { ICreateWorkoutDiaryEntryRequest, ICreateWorkoutDiaryEntryResponse } from '@/api/type/CreateWorkoutDiaryEntry.type';
import { IUpdateWorkoutDiaryEntryRequest, IUpdateWorkoutDiaryEntryResponse } from '@/api/type/UpdateWorkoutDiaryEntry.type';
import { IGetKanbanBoardResponse } from '@/api/type/GetKanbanBoard.type';
import { IAddKanbanColumnRequest, IAddKanbanColumnResponse } from '@/api/type/AddKanbanColumn.type';
import { ISearchKanbanBoardsResponse } from '@/api/type/SearchKanbanBoards.type';
import { IAddKanbanItemRequest, IAddKanbanItemResponse } from '@/api/type/AddKanbanItem.type';
import { IUpdateKanbanItemRequest, IUpdateKanbanItemResponse } from '@/api/type/UpdateKanbanItem.type';
import { IUpdateKanbanBoardPositionsRequest, IUpdateKanbanBoardPositionsResponse } from '@/api/type/UpdateKanbanBoardPositions.type';
import { IUpdateKanbanColumnRequest, IUpdateKanbanColumnResponse } from '@/api/type/UpdateKanbanColumn.type';
import { IUpdateKanbanBoardRequest, IUpdateKanbanBoardResponse } from '@/api/type/UpdateKanbanBoard.type';
import { IGetDashboardResponse } from '@/api/type/GetDashboard.type';
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

        calendar: {
           
            async searchEntries(startAt: Dayjs, endAt: Dayjs): Promise<Array<ICalendarEntry> | Error> {
                const queryParams = new Map<string, string>();
                queryParams.set('startAt', startAt.toISOString());
                queryParams.set('endAt', endAt.toISOString());

                const response = await apiClient.get<ISearchCalendarEntriesResponse>({
                    url: '/calendar/entries',
                    queryParams: queryParams,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                return response.entries.map(x => ({
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
            }): Promise<ICalendarEntry | Error> {

                const response = await apiClient.post<IAddCalendarEntryResponse>({
                    url: '/calendar/entry',
                    body: {
                        description: request.description,
                        startAt: request.startAt.toISOString(),
                        endAt: request.endAt.toISOString(),
                        recurringPeriod: mapApiRecurringPeriod(request.recurringPeriod),
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const entry = response.calendarEntry;

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
            async get(): Promise<IDashboard | Error> {
                const response = await apiClient.get<IGetDashboardResponse>({
                    url: '/dashboard',
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const items = response.items;

                return {
                    items: items.map(x => ({
                        reference: x.reference,
                        title: x.title,
                        type: dashboardItemTypeMapper.map(x.type),
                        createdAt: dayjs(x.createdAt),
                    })),
                };
            },
        },

        notes: {
            async search(): Promise<Array<INote> | Error> {
                const response = await apiClient.get<ISearchNotesResponse>({
                    url: '/notes',
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const notes = response.notes;

                return notes.map(x => ({
                    reference: x.reference,
                    createdAt: dayjs(x.createdAt),
                    title: x.title,
                    content: x.content,
                }));
            },

            async getNote(reference: string): Promise<INote | Error> {
                const response = await apiClient.get<IGetNoteResponse>({
                    url: `/note/${reference}`,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const note = response.note;

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
            }): Promise<INote | Error> {
                const response = await apiClient.post<ICreateNoteResponse>({
                    url: '/note',
                    body: {
                        title: request.title,
                        content: request.content,
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const note = response.note;

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
            }): Promise<INote | Error> {
                const response = await apiClient.put<IUpdateNoteResponse>({
                    url: `/note/${reference}`,
                    body: {
                        title: request.title,
                        content: request.content,
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const note = response.note;

                return {
                    reference: note.reference,
                    createdAt: dayjs(note.createdAt),
                    title: note.title,
                    content: note.content,
                };
            },

            async deleteNote(reference: string): Promise<void | Error> {
                const response = await apiClient.delete({
                    url: `/note/${reference}`,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;
            },
        },

        lists: {
            async search(): Promise<Array<IList> | Error> {
                const response = await apiClient.get<GetListsResponse>({
                    url: '/listum/lists',
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const lists = response.lists;

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

            async getListByReference(reference: string): Promise<IList | Error> {
                const response = await apiClient.get<IGetListByReference>({
                    url: `/listum/list/${reference}`,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const list = response.list;

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
            }): Promise<IList | Error> {
                const response = await apiClient.post<ICreateListResponse>({
                    url: '/listum/list',
                    body: {
                        title: request.title,
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const list = response.list;

                return {
                    reference: list.reference,
                    createdAt: dayjs(list.createdAt),
                    title: list.title,
                    items: [],
                };
            },

            async updateList(reference: string, request: {
                title: string;
            }): Promise<IList | Error> {
                const response = await apiClient.put<IUpdateListResponse>({
                    url: `/listum/list/${reference}`,
                    body: {
                        title: request.title,
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const list = response.list;

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

            async reorderList(reference: string, items: Array<IListItem>): Promise<void | Error> {
                const order: Record<string, number> = {};
                for (let index = 0; index < items.length; ++index) {
                    order[items[index].reference] = index;
                }
                
                const response = await apiClient.post({
                    url: `/listum/list/${reference}/reorder`,
                    body: {
                        order,
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;
            },

            async addListItem(listReference: string, request: {
                content: string;
            }): Promise<IListItem | Error> {
                const response = await apiClient.post<ICreateListItemResponse>({
                    url: `/listum/list/${listReference}/item`,
                    body: {
                        content: request.content,
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const listItem = response.listItem;

                return {
                    reference: listItem.reference,
                    createdAt: dayjs(listItem.createdAt),
                    content: listItem.content,
                    listOrder: listItem.listOrder,
                };
            },

            async updateListItem(listReference: string, itemReference: string, request: {
                content: string;
            }): Promise<IListItem | Error> {
                const response = await apiClient.put<IUpdateListItemResponse>({
                    url: `/listum/list/${listReference}/item/${itemReference}`,
                    body: {
                        content: request.content,
                    },
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const listItem = response.listItem;

                return {
                    reference: listItem.reference,
                    createdAt: dayjs(listItem.createdAt),
                    content: listItem.content,
                    listOrder: listItem.listOrder,
                };
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

        kanban: {
            async search(): Promise<Array<IKanbanBoard> | Error> {
                const response = await apiClient.get<ISearchKanbanBoardsResponse>({
                    url: '/kanban/search',
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const kanbanBoards = response.kanbanBoards;

                return kanbanBoards.map(x => ({
                    reference: x.reference,
                    createdAt: dayjs(x.createdAt),
                    title: x.title,
                    columns: [],
                }));
            },

            async getBoard(reference: string): Promise<IKanbanBoard | Error> {
                const response = await apiClient.get<IGetKanbanBoardResponse>({
                    url: `/kanban/board/${reference}`,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const kanbanBoard = response.kanbanBoard;

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

            async updateBoard(reference: string, request: IUpdateKanbanBoardRequest): Promise<IKanbanBoard | Error> {
                const response = await apiClient.put<IUpdateKanbanBoardResponse>({
                    url: `/kanban/board/${reference}`,
                    body: request,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const kanbanBoard = response.kanbanBoard;

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

            async updateBoardPositions(boardReference: string, request: IUpdateKanbanBoardPositionsRequest): Promise<void | Error> {
                const response = await apiClient.put<IUpdateKanbanBoardPositionsResponse>({
                    url: `/kanban/board/${boardReference}/positions`,
                    body: request,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;
            },

            async addColumn(reference: string, request: IAddKanbanColumnRequest): Promise<IKanbanColumn | Error> {
                const response = await apiClient.post<IAddKanbanColumnResponse>({
                    url: `/kanban/board/${reference}/column`,
                    body: request,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const kanbanColumn = response.kanbanColumn;

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

            async updateColumn(boardReference: string, columnReference: string, request: IUpdateKanbanColumnRequest): Promise<IKanbanColumn | Error> {
                const response = await apiClient.put<IUpdateKanbanColumnResponse>({
                    url: `/kanban/board/${boardReference}/column/${columnReference}`,
                    body: request,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const kanbanColumn = response.kanbanColumn;

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

            async deleteColumn(boardReference: string, columnReference: string): Promise<void | Error> {
                const response = await apiClient.delete({
                    url: `/kanban/board/${boardReference}/column/${columnReference}`,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;
            },

            async addItem(boardReference: string, columnReference: string, request: IAddKanbanItemRequest): Promise<IKanbanItem | Error> {
                const response = await apiClient.post<IAddKanbanItemResponse>({
                    url: `/kanban/board/${boardReference}/column/${columnReference}/item`,
                    body: request,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const kanbanItem = response.kanbanItem;

                return {
                    reference: kanbanItem.reference,
                    createdAt: dayjs(kanbanItem.createdAt),
                    content: kanbanItem.content,
                    position: kanbanItem.position,
                };
            },

            async updateItem(boardReference: string, columnReference: string, itemReference: string, request: IUpdateKanbanItemRequest): Promise<IKanbanItem | Error> {
                const response = await apiClient.put<IUpdateKanbanItemResponse>({
                    url: `/kanban/board/${boardReference}/column/${columnReference}/item/${itemReference}`,
                    body: request,
                    auth: {
                        required: true,
                        use: true,
                    },
                });

                if (response instanceof Error)
                    return response;

                const kanbanItem = response.kanbanItem;

                return {
                    reference: kanbanItem.reference,
                    createdAt: dayjs(kanbanItem.createdAt),
                    content: kanbanItem.content,
                    position: kanbanItem.position,
                };
            },

            async deleteItem(boardReference: string, columnReference: string, itemReference: string): Promise<void | Error> {
                const response = await apiClient.delete({
                    url: `/kanban/board/${boardReference}/column/${columnReference}/item/${itemReference}`,
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
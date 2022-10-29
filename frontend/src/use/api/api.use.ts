import dayjs, { Dayjs } from 'dayjs';

import { CalendarRecurringPeriod, ICalendarEntry } from '@/model/CalendarEntry.model';
import { INote } from '@/model/Note.model';

import { IApiResponse } from '@/use/api/type/ApiResponse.type';

import { ISearchCalendarEntriesResponse } from '@/use/api/type/SearchCalendarEntries.type';
import { IAddCalendarEntryResponse } from '@/use/api/type/AddCalendarEntry.type';
import { ISearchNotesResponse } from '@/use/api/type/SearchNotes.type';
import { IGetNoteResponse } from '@/use/api/type/GetNote.type';
import { ICreateNoteResponse } from '@/use/api/type/CreateNote.type';
import { IUpdateNoteResponse } from '@/use/api/type/UpdateNote.type';
import { IGetListByReference } from './type/GetListByReference.type';
import { IListum } from '@/model/Listum.model';

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
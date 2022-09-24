import { IApiNote } from '@/use/api/type/ApiNote.type';

export interface IUpdateNoteRequest {
    title: string;
    content: string;
}

export interface IUpdateNoteResponse {
    note: IApiNote;
}
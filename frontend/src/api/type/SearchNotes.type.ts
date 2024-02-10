import { IApiNote } from '@/api/type/ApiNote.type';

export interface ISearchNotesResponse {
    notes: Array<IApiNote>;
}
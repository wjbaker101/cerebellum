import { IApiNote } from '@/use/api/type/ApiNote.type';

export interface ISearchNotesResponse {
    notes: Array<IApiNote>;
}
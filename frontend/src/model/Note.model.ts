import { Dayjs } from 'dayjs';

export interface INote {
    reference: string;
    createdAt: Dayjs;
    title: string;
    content: string;
}
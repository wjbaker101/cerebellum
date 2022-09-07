import { Dayjs } from 'dayjs';

export interface ITag {
    reference: string;
    name: string;
    createdAt: Dayjs;
}
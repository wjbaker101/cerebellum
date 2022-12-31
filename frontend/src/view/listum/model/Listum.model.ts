import { Dayjs } from 'dayjs';

export interface IList {
    reference: string;
    createdAt: Dayjs;
    title: string;
    items: Array<IListItem>;
}

export interface IListItem {
    reference: string;
    createdAt: Dayjs;
    content: string;
    listOrder: number;
}
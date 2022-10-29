import { Dayjs } from 'dayjs';

export interface IListum {
    reference: string;
    createdAt: Dayjs;
    title: string;
    items: Array<IListumItem>;
}

export interface IListumItem {
    reference: string;
    createdAt: Dayjs;
    content: string;
    listOrder: number;
}
import { Dayjs } from 'dayjs';

export interface IKanbanBoard {
    reference: string;
    createdAt: Dayjs;
    title: string;
    columns: Array<IKanbanColumn>;
}

export interface IKanbanColumn {
    reference: string;
    createdAt: Dayjs;
    title: string;
    position: number;
    items: Array<IKanbanItem>;
}

export interface IKanbanItem {
    reference: string;
    createdAt: Dayjs;
    content: string;
    position: number;
}
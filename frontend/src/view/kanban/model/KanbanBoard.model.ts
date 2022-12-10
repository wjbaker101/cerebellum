import { Dayjs } from 'dayjs';

export interface IKanbanBoard {
    createdAt: Dayjs;
    title: string;
    columns: Array<IKanbanColumn>;
}

export interface IKanbanColumn {
    createdAt: Dayjs;
    title: string;
    items: Array<IKanbanItem>;
}

export interface IKanbanItem {
    createdAt: Dayjs;
    content: string;
}
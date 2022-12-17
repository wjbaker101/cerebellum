import { IApiKanbanColumn } from './ApiKanban.type';

export interface IUpdateKanbanColumnRequest {
    title: string;
}

export interface IUpdateKanbanColumnResponse {
    kanbanColumn: IApiKanbanColumn;
}
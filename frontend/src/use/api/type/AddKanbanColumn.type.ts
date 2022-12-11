import { IApiKanbanColumn } from './ApiKanban.type';

export interface IAddKanbanColumnRequest {
    title: string;
}

export interface IAddKanbanColumnResponse {
    kanbanColumn: IApiKanbanColumn;
}
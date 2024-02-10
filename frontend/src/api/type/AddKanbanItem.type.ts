import { IApiKanbanItem } from './ApiKanban.type';

export interface IAddKanbanItemRequest {
    content: string;
}

export interface IAddKanbanItemResponse {
    kanbanItem: IApiKanbanItem;
}
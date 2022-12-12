import { IApiKanbanItem } from './ApiKanban.type';

export interface IUpdateKanbanItemRequest {
    content: string;
    columnReference: string;
}

export interface IUpdateKanbanItemResponse {
    kanbanItem: IApiKanbanItem;
}
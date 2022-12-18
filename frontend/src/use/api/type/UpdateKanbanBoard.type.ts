import { IApiKanbanBoard } from './ApiKanban.type';

export interface IUpdateKanbanBoardRequest {
    title: string;
}

export interface IUpdateKanbanBoardResponse {
    kanbanBoard: IApiKanbanBoard;
}
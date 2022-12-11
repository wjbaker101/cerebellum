import { IApiKanbanBoard } from './ApiKanban.type';

export interface ISearchKanbanBoardsResponse {
    kanbanBoards: Array<IApiKanbanBoard>;
}
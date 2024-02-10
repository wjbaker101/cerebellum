export interface IUpdateKanbanBoardPositionsRequest {
    columns: Record<string, Column>;
}

interface Column {
    position: number;
    items: Record<string, number>;
}

export interface IUpdateKanbanBoardPositionsResponse {
}
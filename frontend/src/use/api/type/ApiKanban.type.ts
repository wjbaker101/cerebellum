export interface IApiKanbanBoard {
    reference: string;
    createdAt: string;
    title: string;
    columns: Array<IApiKanbanColumn>;
}

export interface IApiKanbanColumn {
    reference: string;
    createdAt: string;
    title: string;
    position: number;
    items: Array<IApiKanbanItem>;
}

export interface IApiKanbanItem {
    reference: string;
    createdAt: string;
    content: string;
    position: number;
}
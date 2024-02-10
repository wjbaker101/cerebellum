export interface IApiListum {
    reference: string;
    createdAt: string;
    title: string;
    items: Array<IApiListumItem>;
}

export interface IApiListumItem {
    reference: string;
    createdAt: string;
    content: string;
    listOrder: number;
}
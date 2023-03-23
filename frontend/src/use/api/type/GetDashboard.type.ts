export interface IGetDashboardResponse {
    readonly items: Array<IApiDashboardItem>;
}

interface IApiDashboardItem {
    readonly reference: string;
    readonly title: string;
    readonly type: number;
    readonly createdAt: string;
}
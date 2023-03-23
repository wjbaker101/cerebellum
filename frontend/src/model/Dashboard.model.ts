import { Dayjs } from 'dayjs';

export interface IDashboard {
    readonly items: Array<IDashboardItem>;
}

interface IDashboardItem {
    readonly reference: string;
    readonly title: string;
    readonly type: DashboardItemType;
    readonly createdAt: Dayjs;
}

export type DashboardItemType = 'unknown' | 'note' | 'list' | 'kanban-board';
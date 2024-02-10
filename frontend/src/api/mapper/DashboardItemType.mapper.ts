import { DashboardItemType } from '@/model/Dashboard.model';

export const dashboardItemTypeMapper = {

    map(type: number): DashboardItemType {
        switch (type) {
            case 1:
                return 'note'
            case 2:
                return 'list';
            case 3:
                return 'kanban-board';
            default:
                return 'unknown';
        }
    },

};
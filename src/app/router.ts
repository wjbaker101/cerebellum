import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

import CalendarView from '@/view/calendar/CalendarView.vue';
import DashboardView from '@/view/dashboard/DashboardView.vue';

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        component: DashboardView,
    },
    {
        path: '/calendar',
        component: CalendarView,
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export { router };
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

import DashboardView from '@/view/dashboard/DashboardView.vue';
import WorkoutDiaryView from '@/view/workout-diary/WorkoutDiaryView.vue';
import LogInView from '@/view/log-in/LogIn.view.vue';

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        component: DashboardView,
    },
    {
        path: '/workout-diary',
        component: WorkoutDiaryView,
    },
    {
        path: '/workout-diary/entry/:entryReference',
        component: WorkoutDiaryView,
    },
    {
        path: '/log-in',
        component: LogInView,
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export { router };
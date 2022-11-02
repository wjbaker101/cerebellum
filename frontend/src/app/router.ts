import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

import CalendarView from '@/view/calendar/CalendarView.vue';
import DashboardView from '@/view/dashboard/DashboardView.vue';
import ListumView from '@/view/listum/ListumView.vue';
import ListView from '@/view/listum/ListView.vue';
import NotesView from '@/view/notes/NotesView.vue';
import NoteView from '@/view/notes/NoteView.vue';
import WorkoutDiaryView from '@/view/workout-diary/WorkoutDiaryView.vue';

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        component: DashboardView,
    },
    {
        path: '/calendar',
        component: CalendarView,
    },
    {
        path: '/notes/:noteReference',
        component: NoteView,
    },
    {
        path: '/notes',
        component: NotesView,
    },
    {
        path: '/listum/:listumReference',
        component: ListView,
    },
    {
        path: '/listum',
        component: ListumView,
    },
    {
        path: '/workout-diary',
        component: WorkoutDiaryView,
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export { router };
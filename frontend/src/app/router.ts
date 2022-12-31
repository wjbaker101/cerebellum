import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

import CalendarView from '@/view/calendar/CalendarView.vue';
import DashboardView from '@/view/dashboard/DashboardView.vue';
import KanbanView from '@/view/kanban/KanbanView.vue';
import KanbanBoardView from '@/view/kanban/KanbanBoardView.vue';
import ListsView from '@/view/lists/ListsView.vue';
import ListView from '@/view/lists/ListView.vue';
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
        path: '/lists/:listReference',
        component: ListView,
    },
    {
        path: '/lists',
        component: ListsView,
    },
    {
        path: '/workout-diary',
        component: WorkoutDiaryView,
    },
    {
        path: '/kanban',
        component: KanbanView,
    },
    {
        path: '/kanban/:kanbanReference',
        component: KanbanBoardView,
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export { router };
<template>
    <ViewComponent class="kanban-view flex flex-vertical gap" :heading="kanbanBoard.title">
        <template v-slot:title v-if="isEditingTitle">
            <h1>
                <input ref="titleInput" type="text" v-model="kanbanTitle" @keypress.enter="onTitleConfirm">
            </h1>
        </template>
        <template v-slot:header>
            <ButtonComponent class="primary" @click="onAddColumn">
                <IconComponent icon="plus" gap="right" />
                <span>New Column</span>
            </ButtonComponent>
        </template>
        <div class="kanban-board flex gap">
            <div class="kanban-column" v-for="column in kanbanBoard.columns">
                <header class="flex gap-small align-items-center">
                    <h2>{{ column.title }}</h2>
                    <div class="flex-auto">
                        <ButtonComponent class="primary mini" @click="onAddItem(column)">
                            <IconComponent icon="plus" />
                            <span>Add Item</span>
                        </ButtonComponent>
                    </div>
                </header>
                <Sortable
                    :list="column.items"
                    tag="div"
                    item-key="createdAt"
                    :options="{
                        draggable: '.draggable',
                        animation: 150,
                        ghostClass: 'ghost',
                        dragClass: 'drag',
                    }"
                >
                    <template #item="{ element }">
                        <KanbanItemComponent :kanbanItem="element" :key="element.createdAt" />
                    </template>
                </Sortable>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import dayjs from 'dayjs';
import { Sortable } from 'sortablejs-vue3';

import KanbanItemComponent from '@/view/kanban/component/KanbanItemComponent.vue';

import { IKanbanBoard, IKanbanColumn } from '@/view/kanban/model/KanbanBoard.model';

const kanbanBoard = ref<IKanbanBoard>({
    createdAt: dayjs(),
    title: 'Test Kanban Board',
    columns: [
        {
            createdAt: dayjs(),
            title: 'Backlog',
            items: [],
        },
        {
            createdAt: dayjs(),
            title: 'In Progress',
            items: [],
        },
        {
            createdAt: dayjs(),
            title: 'Complete',
            items: [],
        },
    ],
});

const isEditingTitle = ref<boolean>(false);

const kanbanTitle = ref<string>(kanbanBoard.value.title);

const onTitleConfirm = function(): void {};

const onAddColumn = function (): void {
    kanbanBoard.value.columns.push({
        createdAt: dayjs(),
        title: 'New Column',
        items: [],
    });
};

const onAddItem = function (column: IKanbanColumn): void {
    column.items.push({
        createdAt: dayjs(),
        content: 'New Item ' + (Math.random() * 1000).toFixed(0),
    });
};
</script>

<style lang="scss">
@use '~@wjb/styling/variables' as *;

.kanban-view {

    .kanban-board {
        margin: -1rem;
        padding: 1rem;
        overflow-x: auto;
    }

    .kanban-column {
        flex: 0 0 350px;
        padding: 1rem;
        border-top: 4px solid var(--wjb-secondary);
        border-radius: var(--wjb-border-radius);

        @include shadow-large();

        & > header {
            padding-bottom: 1rem;

            h2 {
                margin: 0;
            }
        }
    }
}
</style>
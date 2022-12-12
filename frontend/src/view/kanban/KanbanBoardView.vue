<template>
    <ViewComponent v-if="kanbanBoard" class="kanban-board-view flex flex-vertical gap" :heading="kanbanBoard.title">
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
                    item-key="reference"
                    :options="{
                        draggable: '.draggable',
                        animation: 150,
                        ghostClass: 'ghost',
                        dragClass: 'drag',
                        group: 'items',
                    }"
                    @add="onAdd"
                    @remove="onRemove"
                >
                    <template #item="{ element }">
                        <KanbanItemComponent :kanbanItem="element" :key="element.reference" />
                    </template>
                </Sortable>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import dayjs from 'dayjs';
import { Sortable } from 'sortablejs-vue3';

import KanbanItemComponent from '@/view/kanban/component/KanbanItemComponent.vue';

import { useApi } from '@/use/api/api.use';

import { IKanbanBoard, IKanbanColumn } from '@/view/kanban/model/KanbanBoard.model';

const api = useApi();
const route = useRoute();

const kanbanBoardReference = route.params.kanbanReference as string;

const kanbanBoard = ref<IKanbanBoard | null>(null);

const isEditingTitle = ref<boolean>(false);

const kanbanTitle = ref<string>(kanbanBoard.value?.title ?? '');

const onTitleConfirm = function(): void {};

const onAddColumn = async function (): Promise<void> {
    const column = await api.kanban.addColumn(kanbanBoardReference, {
        title: 'New Column',
    });

    kanbanBoard.value?.columns.push(column);
};

const onAddItem = async function (column: IKanbanColumn): Promise<void> {
    if (kanbanBoard.value === null)
        return;

    const result = await api.kanban.addItem(kanbanBoard.value.reference, column.reference, {
        content: 'new item',
    });

    column.items.push(result);
};

const onAdd = function (event: any): void { console.log(event); };

const onRemove = function (event: any): void { console.log(event); };

onMounted(async () => {
    const result = await api.kanban.getBoard(kanbanBoardReference);

    kanbanBoard.value = result;
});
</script>

<style lang="scss">
@use '~@wjb/styling/variables' as *;

.kanban-board-view {

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
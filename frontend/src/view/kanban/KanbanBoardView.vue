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
        <VueSortable
            class="kanban-board flex gap"
            :list="kanbanBoard.columns"
            tag="div"
            item-key="reference"
            :options="{
                draggable: '.draggable',
                animation: 150,
                ghostClass: 'ghost',
                dragClass: 'drag',
                group: 'columns',
            }"
            @end="onEnd"
        >
            <template #item="{ element }">
                <KanbanColumnComponent :key="element.reference" :kanbanBoard="kanbanBoard" :kanbanColumn="element" />
            </template>
        </VueSortable>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import { Sortable as VueSortable } from 'sortablejs-vue3';
import Sortable from 'sortablejs';

import KanbanColumnComponent from './component/KanbanColumnComponent.vue';

import { useApi } from '@/use/api/api.use';
import { recordHelper } from '@/helper/record.helper';

import { IKanbanBoard } from '@/view/kanban/model/KanbanBoard.model';

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

const onEnd = async function (event: Sortable.SortableEvent): Promise<void> {
    if (kanbanBoard.value === null)
        return;

    if (event.oldIndex === undefined || event.newIndex === undefined)
        return;

    const column = kanbanBoard.value.columns.splice(event.oldIndex, 1)[0];
    kanbanBoard.value.columns.splice(event.newIndex, 0, column);

    await api.kanban.updateBoardPositions(kanbanBoard.value.reference, {
        columns: recordHelper.toRecord(kanbanBoard.value.columns, col => col.reference, (col, columnIndex) => ({
            position: columnIndex,
            items: recordHelper.toRecord(col.items, itm => itm.reference, (itm, itemIndex) => itemIndex),
        })),
    });
};

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
            h2 {
                margin: 0;
            }
        }

        .items {
            margin: -1rem;
            padding: 1rem;
            overflow-y: auto;
        }
    }
}
</style>
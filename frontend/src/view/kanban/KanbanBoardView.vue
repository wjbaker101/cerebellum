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
                <VueSortable
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
                    :data-reference="column.reference"
                    @end="onEnd"
                >
                    <template #item="{ element }">
                        <KanbanItemComponent :kanbanItem="element" :key="element.reference" />
                    </template>
                </VueSortable>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import { Sortable as VueSortable } from 'sortablejs-vue3';
import Sortable from 'sortablejs';

import KanbanItemComponent from '@/view/kanban/component/KanbanItemComponent.vue';

import { recordHelper } from '@/helper/record.helper';
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

const onEnd = async function (event: Sortable.SortableEvent): Promise<void> {
    if (kanbanBoard.value === null)
        return;

    const fromColumnReference = event.from.getAttribute('data-reference');
    const toColumnReference = event.to.getAttribute('data-reference');

    const fromColumn = kanbanBoard.value.columns.find(x => x.reference === fromColumnReference);
    const toColumn = kanbanBoard.value.columns.find(x => x.reference === toColumnReference);
    if (!fromColumn || !toColumn)
        return;

    if (event.oldIndex === undefined || event.newIndex === undefined)
        return;

    const item = fromColumn.items.splice(event.oldIndex, 1)[0];
    toColumn.items.splice(event.newIndex, 0, item);

    if (fromColumn.reference !== toColumn.reference)
        event.item.remove();

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
            padding-bottom: 1rem;

            h2 {
                margin: 0;
            }
        }
    }
}
</style>
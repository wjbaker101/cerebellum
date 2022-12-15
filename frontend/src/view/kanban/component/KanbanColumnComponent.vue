<template>
    <div class="kanban-column flex flex-vertical gap">
        <header class="flex gap-small align-items-center flex-auto">
            <h2>{{ kanbanColumn.title }}</h2>
            <div class="flex-auto">
                <ButtonComponent class="primary mini" @click="onAddItem(kanbanColumn)">
                    <IconComponent icon="plus" />
                    <span>Add Item</span>
                </ButtonComponent>
            </div>
        </header>
        <VueSortable
            class="items"
            :list="kanbanColumn.items"
            tag="div"
            item-key="reference"
            :options="{
                draggable: '.draggable',
                animation: 150,
                ghostClass: 'ghost',
                dragClass: 'drag',
                group: 'items',
            }"
            :data-reference="kanbanColumn.reference"
            @end="onEnd"
        >
            <template #item="{ element }">
                <KanbanItemComponent
                    :boardReference="kanbanBoard.reference"
                    :kanbanColumn="kanbanColumn"
                    :kanbanItem="element"
                    :key="element.reference"
                />
            </template>
        </VueSortable>
    </div>
</template>

<script setup lang="ts">
import { Sortable as VueSortable } from 'sortablejs-vue3';
import Sortable from 'sortablejs';

import KanbanItemComponent from '@/view/kanban/component/KanbanItemComponent.vue';

import { recordHelper } from '@/helper/record.helper';
import { useApi } from '@/use/api/api.use';

import { IKanbanBoard, IKanbanColumn } from '@/view/kanban/model/KanbanBoard.model';

const props = defineProps<{
    kanbanBoard: IKanbanBoard;
    kanbanColumn: IKanbanColumn;
}>();

const api = useApi();

const onAddItem = async function (column: IKanbanColumn): Promise<void> {
    const result = await api.kanban.addItem(props.kanbanBoard.reference, column.reference, {
        content: 'new item',
    });

    column.items.push(result);
};

const onEnd = async function (event: Sortable.SortableEvent): Promise<void> {
    const fromColumnReference = event.from.getAttribute('data-reference');
    const toColumnReference = event.to.getAttribute('data-reference');

    const fromColumn = props.kanbanBoard.columns.find(x => x.reference === fromColumnReference);
    const toColumn = props.kanbanBoard.columns.find(x => x.reference === toColumnReference);
    if (!fromColumn || !toColumn)
        return;

    if (event.oldIndex === undefined || event.newIndex === undefined)
        return;

    const item = fromColumn.items.splice(event.oldIndex, 1)[0];
    toColumn.items.splice(event.newIndex, 0, item);

    if (fromColumn.reference !== toColumn.reference)
        event.item.remove();

    await api.kanban.updateBoardPositions(props.kanbanBoard.reference, {
        columns: recordHelper.toRecord(props.kanbanBoard.columns, col => col.reference, (col, columnIndex) => ({
            position: columnIndex,
            items: recordHelper.toRecord(col.items, itm => itm.reference, (itm, itemIndex) => itemIndex),
        })),
    });
};
</script>

<style lang="scss">
</style>
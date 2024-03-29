<template>
    <div class="kanban-column flex flex-vertical gap" :class=" { 'draggable': isDraggable }">
        <header class="flex gap-small align-items-center flex-auto">
            <h2>{{ kanbanColumn.title }}</h2>
            <div class="flex-auto">
                <ButtonComponent class="primary mini" @click="onAddItem(kanbanColumn)">
                    <IconComponent icon="plus" />
                    <span>Add Item</span>
                </ButtonComponent>
            </div>
            <div class="flex-auto">
                <ButtonComponent class="mini" @click="onOpenDetails">
                    <IconComponent icon="menu" />
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
                    @contentFocus="onItemContentFocus"
                    @contentBlur="onItemContentBlur"
                />
            </template>
        </VueSortable>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { Sortable as VueSortable } from 'sortablejs-vue3';
import Sortable from 'sortablejs';

import KanbanItemComponent from '@/view/kanban/component/KanbanItemComponent.vue';
import KanbanColumnModalComponent, { IKanbanColumnModalProps } from '@/view/kanban/modal/KanbanColumn.modal.component.vue';

import { recordHelper } from '@/helper/record.helper';
import { useApi } from '@/api/api.use';
import { useModal } from '@wjb/vue/use/modal.use';

import { IKanbanBoard, IKanbanColumn } from '@/view/kanban/model/KanbanBoard.model';

const props = defineProps<{
    kanbanBoard: IKanbanBoard;
    kanbanColumn: IKanbanColumn;
}>();

const api = useApi();
const modal = useModal();

const isDraggable = ref<boolean>(true);

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

const onOpenDetails = function (): void {
    modal.show<IKanbanColumnModalProps>({
        component: KanbanColumnModalComponent,
        componentProps: {
            kanbanBoard: props.kanbanBoard,
            kanbanColumn: props.kanbanColumn,
        },
    });
};

const onItemContentFocus = function (): void {
    isDraggable.value = false;
};

const onItemContentBlur = function (): void {
    isDraggable.value = true;
};
</script>

<style lang="scss">
</style>@/api/api.use
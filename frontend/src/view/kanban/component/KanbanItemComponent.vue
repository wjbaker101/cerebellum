<template>
    <div class="kanban-item-component draggable" @click="onClick" :data-reference="kanbanItem.reference">
        {{ kanbanItem.content }}
    </div>
</template>

<script setup lang="ts">
import { useModal } from '@wjb/vue/use/modal.use';

import { IKanbanColumn, IKanbanItem } from '@/view/kanban/model/KanbanBoard.model';
import KanbanItemModalComponent from '@/view/kanban/modal/KanbanItem.modal.component.vue';

const props = defineProps<{
    boardReference: string;
    kanbanColumn: IKanbanColumn;
    kanbanItem: IKanbanItem;
}>();

const modal = useModal();

const onClick = function (): void {
    modal.show({
        component: KanbanItemModalComponent,
        componentProps: {
            boardReference: props.boardReference,
            kanbanColumn: props.kanbanColumn,
            kanbanItem: props.kanbanItem,
        },
    });
};
</script>

<style lang="scss">
@use '~@wjb/styling/variables' as *;

.kanban-item-component {
    padding: 0.5rem;
    border-radius: var(--wjb-border-radius);
    cursor: pointer;

    @include shadow-small();

    & + .kanban-item {
        margin-top: 0.25rem;
    }
}
</style>
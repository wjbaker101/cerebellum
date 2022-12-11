<template>
    <div class="kanban-item-component draggable" @click="onClick">
        {{ kanbanItem.content }}
    </div>
</template>

<script setup lang="ts">
import { useModal } from '@wjb/vue/use/modal.use';

import { IKanbanItem } from '@/view/kanban/model/KanbanBoard.model';
import KanbanItemModalComponent from '@/view/kanban/modal/KanbanItem.modal.component.vue';

const props = defineProps<{
    kanbanItem: IKanbanItem;
}>();

const modal = useModal();

const onClick = function (): void {
    modal.show({
        component: KanbanItemModalComponent,
        componentProps: {
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
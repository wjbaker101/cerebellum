<template>
    <div class="kanban-item-component draggable">
        {{ kanbanItem.content }}
    </div>
</template>

<script setup lang="ts">
import { IKanbanItem } from '@/view/kanban/model/KanbanBoard.model';

defineProps<{
    kanbanItem: IKanbanItem;
}>();
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
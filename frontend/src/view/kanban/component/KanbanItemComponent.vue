<template>
    <div class="flex gap-small kanban-item-component" :class=" { 'draggable': isDraggable }" :data-reference="kanbanItem.reference">
        <HiddenTextboxComponent class="content-input" v-model="itemContent" @finish="onFinish" @focus="onFocus" @blur="onBlur" />
        <ButtonComponent class="flex-auto details-button mini" @click="onClick">
            <IconComponent icon="menu" />
        </ButtonComponent>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { useApi } from '@/api/api.use';
import { useModal } from '@/composables/modal.use';

import { IKanbanColumn, IKanbanItem } from '@/view/kanban/model/KanbanBoard.model';
import KanbanItemModalComponent, { IKanbanItemModalProps } from '@/view/kanban/modal/KanbanItem.modal.component.vue';
import HiddenTextboxComponent from '@/component/HiddenTextboxComponent.vue';
import ButtonComponent from '@/component/ButtonComponent.vue';
import IconComponent from '@/component/IconComponent.vue';

const props = defineProps<{
    boardReference: string;
    kanbanColumn: IKanbanColumn;
    kanbanItem: IKanbanItem;
}>();

const emit = defineEmits(['contentFocus', 'contentBlur']);

const api = useApi();
const modal = useModal();

const itemContent = ref<string>(props.kanbanItem.content);

const isDraggable = ref<boolean>(true);

const onClick = function (): void {
    modal.show<IKanbanItemModalProps>({
        component: KanbanItemModalComponent,
        componentProps: {
            boardReference: props.boardReference,
            kanbanColumn: props.kanbanColumn,
            kanbanItem: props.kanbanItem,
        },
    });
};

const onFinish = async function (content: string): Promise<void> {
    const result = await api.kanban.updateItem(props.boardReference, props.kanbanColumn.reference, props.kanbanItem.reference, {
        content,
        columnReference: props.kanbanColumn.reference,
    });

    props.kanbanItem.content = result.content;
};

const onFocus = function (): void {
    isDraggable.value = false;
    emit('contentFocus');
};

const onBlur = function (): void {
    isDraggable.value = true;
    emit('contentBlur');
};
</script>

<style lang="scss">
@use '@/styling/variables' as *;

.kanban-item-component {
    padding: 0.5rem;
    border-radius: var(--wjb-border-radius);
    cursor: pointer;

    & + .kanban-item {
        margin-top: 0.25rem;
    }

    .details-button {
        opacity: 0;
    }

    &:hover {
        .details-button {
            opacity: 1;
        }
    }

    .content-input {
        &:focus {
            & + .details-button {
                opacity: 1;
            }
        }
    }
}
</style>@/api/api.use
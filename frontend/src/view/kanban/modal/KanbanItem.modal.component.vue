<template>
    <div class="kanban-item-modal-component">
        <h2>Edit Item</h2>
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Content">
                    <input type="text" v-model="form.content">
                </FormInputComponent>
            </FormSectionComponent>
            <FormSectionComponent class="flex gap-small">
                <ButtonComponent class="primary flex-auto" @click="onConfirm">
                    <IconComponent icon="tick" gap="right" />
                    <span>Confirm</span>
                </ButtonComponent>
                <DeleteButtonComponent class="flex-auto" @delete="onDelete" />
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { useApi } from '@/use/api/api.use';
import { useModal } from '@wjb/vue/use/modal.use';

import { IKanbanColumn, IKanbanItem } from '@/view/kanban/model/KanbanBoard.model';

export interface IKanbanItemModalProps {
    boardReference: string;
    kanbanColumn: IKanbanColumn;
    kanbanItem: IKanbanItem;
}
const props = defineProps<IKanbanItemModalProps>();

interface IKanbanItemForm {
    content: string;
}

const api = useApi();
const modal = useModal();

const form = ref<IKanbanItemForm>({
    content: props.kanbanItem.content,
});

const onConfirm = function (): void {
    props.kanbanItem.content = form.value.content;
};

const onDelete = async function (): Promise<void> {
    await api.kanban.deleteItem(props.boardReference, props.kanbanColumn.reference, props.kanbanItem.reference);

    props.kanbanColumn.items = props.kanbanColumn.items.filter(x => x !== props.kanbanItem);

    modal.hide();
};
</script>

<style lang="scss">
.kanban-item-modal-component {
    width: 720px;
    max-width: 100%;
}
</style>
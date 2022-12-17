<template>
    <div class="kanban-column-modal-component">
        <h2>Edit Column</h2>
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Title">
                    <input type="text" v-model="form.title">
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

import { IKanbanBoard, IKanbanColumn } from '@/view/kanban/model/KanbanBoard.model';

export interface IKanbanColumnModalProps {
    kanbanBoard: IKanbanBoard;
    kanbanColumn: IKanbanColumn;
}
const props = defineProps<IKanbanColumnModalProps>();

interface IKanbanColumnForm {
    title: string;
}

const api = useApi();
const modal = useModal();

const form = ref<IKanbanColumnForm>({
    title: props.kanbanColumn.title,
});

const onConfirm = async function (): Promise<void> {
    const result = await api.kanban.updateColumn(props.kanbanBoard.reference, props.kanbanColumn.reference, {
        title: form.value.title,
    });

    props.kanbanColumn.title = result.title;
};

const onDelete = async function (): Promise<void> {
    await api.kanban.deleteColumn(props.kanbanBoard.reference, props.kanbanColumn.reference);

    props.kanbanBoard.columns = props.kanbanBoard.columns.filter(x => x !== props.kanbanColumn);

    modal.hide();
};
</script>

<style lang="scss">
.kanban-column-modal-component {
    width: 720px;
    max-width: 100%;
}
</style>
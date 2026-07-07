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
                <ButtonComponent class="flex-auto primary" @click="onConfirm">
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

import { useApi } from '@/api/api.use';
import { useModal } from '@/composables/modal.use';

import { IKanbanColumn, IKanbanItem } from '@/view/kanban/model/KanbanBoard.model';
import FormComponent from '@/component/form/FormComponent.vue';
import FormSectionComponent from '@/component/form/FormSectionComponent.vue';
import FormInputComponent from '@/component/form/FormInputComponent.vue';
import IconComponent from '@/component/IconComponent.vue';
import ButtonComponent from '@/component/ButtonComponent.vue';

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

const onConfirm = async function (): Promise<void> {
    const result = await api.kanban.updateItem(props.boardReference, props.kanbanColumn.reference, props.kanbanItem.reference, {
        content: form.value.content,
        columnReference: props.kanbanColumn.reference,
    });

    props.kanbanItem.content = result.content;
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
</style>@/api/api.use
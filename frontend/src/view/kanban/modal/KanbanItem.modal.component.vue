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
                <DeleteButtonComponent class="flex-auto" />
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { IKanbanItem } from '@/view/kanban/model/KanbanBoard.model';

const props = defineProps<{
    kanbanItem: IKanbanItem;
}>();

interface IKanbanItemForm {
    content: string;
}

const form = ref<IKanbanItemForm>({
    content: props.kanbanItem.content,
});

const onConfirm = function (): void {
    props.kanbanItem.content = form.value.content;
};
</script>

<style lang="scss">
.kanban-item-modal-component {
    width: 720px;
    max-width: 100%;
}
</style>
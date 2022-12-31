<template>
    <div class="list-item-modal-component">
        <h2>Edit List Items</h2>
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Content">
                    <input type="text" v-model="newContent">
                </FormInputComponent>
                <FormInputComponent>
                    <ButtonComponent class="primary" @click="onSubmit">Update</ButtonComponent>
                </FormInputComponent>
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { useModal } from '@wjb/vue/use/modal.use';
import { useApi } from '@/use/api/api.use';

import { IListItem } from '@/view/lists/model/List.model';

export interface IListItemModalProps {
    listReference: string;
    listItem: IListItem;
}
const props = defineProps<IListItemModalProps>();

const api = useApi();
const modal = useModal();

const newContent = ref<string>(props.listItem.content);

const onSubmit = async function (): Promise<void> {
    const response = await api.listum.updateListItem(props.listReference, props.listItem.reference, {
        content: newContent.value,
    });

    props.listItem.content = response.content;

    modal.hide();
};
</script>

<style lang="scss">
.list-item-modal-component {
    width: 650px;
    max-width: 100%;
}
</style>
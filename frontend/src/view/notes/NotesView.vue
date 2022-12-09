<template>
    <ViewComponent class="notes-view" heading="Notes" :no-header="false">
        <template #header>
            <ButtonComponent class="primary" @click="onCreate">
                <IconComponent icon="plus" gap="right" />
                <span>Create</span>
            </ButtonComponent>
        </template>
        <ListComponent :items="notes" itemName="notes" :urlMapper="(x: INote) => `/notes/${x.reference}`">
            <template #item="{ item }">
                {{ item.title }}
            </template>
        </ListComponent>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

import ListComponent from '@/component/ListComponent.vue';

import { useApi } from '@/use/api/api.use';

import { INote } from '@/model/Note.model';

const api = useApi();
const router = useRouter();

const notes = ref<Array<INote> | null>(null);

const onCreate = async function () {
    const note = await api.notes.createNote({
        title: 'New Note',
        content: '',
    });

    notes.value?.push(note);
    router.push({ path: `/notes/${note.reference}`, });
};

onMounted(async () => {
    const response = await api.notes.search();

    notes.value = response;
});
</script>

<style lang="scss">
</style>
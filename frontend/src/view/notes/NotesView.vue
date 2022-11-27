<template>
    <ViewComponent class="notes-view" heading="Notes" :no-header="false">
        <template #header>
            <ButtonComponent class="primary" @click="onCreate">
                <IconComponent icon="plus" gap="right" />
                <span>Create</span>
            </ButtonComponent>
        </template>
        <LoadingComponent itemName="notes" v-if="notes === null" />
        <NoItemsComponent itemName="notes" v-else-if="notes.length === 0" />
        <div v-else>
            <router-link class="note" :to="`/notes/${note.reference}`" :key="note.reference" v-for="note in notes">
                <div>
                    {{ note.title }}
                </div>
            </router-link>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

import LoadingComponent from '@/component/LoadingComponent.vue';
import NoItemsComponent from '@/component/NoItemsComponent.vue';

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
<template>
    <ViewComponent class="notes-view" heading="Notes" :no-header="false">
        <div v-if="notes === null">
            <p>Loading...</p>
        </div>
        <div v-else>
            <router-link class="note" :to="`/notes/${note.reference}`" :key="note.reference" v-for="note in notes">
                <div>
                    {{ note.reference }}
                </div>
            </router-link>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { useApi } from '@/use/api/api.use';
import { INote } from '@/model/Note.model';

const api = useApi();

const notes = ref<Array<INote> | null>(null);

onMounted(async () => {
    const response = await api.notes.search();

    notes.value = response;
});
</script>

<style lang="scss">
</style>
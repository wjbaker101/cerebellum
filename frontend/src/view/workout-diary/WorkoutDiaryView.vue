<template>
    <ViewComponent class="workout-diary-view flex flex-vertical gap" heading="Workout Diary">
        <template v-slot:header>
            <ButtonComponent class="primary" @click="onAddEntry">
                <IconComponent icon="plus" gap="right" />
                <span>New Entry</span>
            </ButtonComponent>
        </template>
        <div class="entry-list-container" v-if="entries !== null">
            <WorkoutEntryComponent v-for="entry in entries" :workoutEntry="entry" />
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import WorkoutEntryComponent from '@/view/workout-diary/component/WorkoutEntry.component.vue';
import WorkoutEntryModalComponent from '@/view/workout-diary/modal/WorkoutEntry.modal.component.vue';

import { useModal } from '@wjb/vue/use/modal.use';
import { useApi } from '@/use/api/api.use';

import { IWorkoutEntry } from './model/WorkoutEntry.model';

const api = useApi();
const modal = useModal();

const entries = ref<Array<IWorkoutEntry> | null>(null);

onMounted(async () => {
    const result = await api.workoutDiary.search();

    entries.value = result;
});

const onAddEntry = function (): void {
    modal.show({
        component: WorkoutEntryModalComponent,
        componentProps: {},
    });
};
</script>

<style lang="scss">
@use '~@wjb/styling/variables' as *;

.workout-diary-view {
    .entry-list-container {
        width: 720px;
        max-width: 100%;
        margin: auto;
    }
}
</style>
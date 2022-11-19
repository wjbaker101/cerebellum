<template>
    <ViewComponent class="workout-diary-view flex flex-vertical gap" heading="Workout Diary">
        <template v-slot:header>
            <ButtonComponent class="primary">
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

import { useApi } from '@/use/api/api.use';

import { IWorkoutEntry } from './model/WorkoutEntry.model';

const api = useApi();

const entries = ref<Array<IWorkoutEntry> | null>(null);

onMounted(async () => {
    const result = await api.workoutDiary.search();

    entries.value = result;
});
</script>

<style lang="scss">
@use '~@wjb/styling/variables' as *;

.workout-diary-view {
    .entry-list-container {
        width: 720px;
        max-width: auto;
        margin: auto;
    }
}
</style>
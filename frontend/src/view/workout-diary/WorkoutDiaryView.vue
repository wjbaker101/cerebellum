<template>
    <ViewComponent class="workout-diary-view flex flex-vertical gap" heading="Workout Diary">
        <template v-slot:header>
            <ButtonComponent class="primary" @click="onAddEntry">
                <IconComponent icon="plus" gap="right" />
                <span>New Entry</span>
            </ButtonComponent>
        </template>
        <ListComponent :items="entries" itemName="entries" removeStyling>
            <template #item="{ item }">
                <WorkoutEntryComponent :workoutEntry="item" />
            </template>
        </ListComponent>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';

import ListComponent from '@/component/ListComponent.vue';
import WorkoutEntryComponent from '@/view/workout-diary/component/WorkoutEntry.component.vue';
import WorkoutEntryModalComponent from '@/view/workout-diary/modal/WorkoutEntry.modal.component.vue';

import { useModal } from '@wjb/vue/use/modal.use';
import { useWorkoutDiary } from '@/view/workout-diary/use/workout-diary.use';

const modal = useModal();
const workoutDiary = useWorkoutDiary();

const entries = workoutDiary.entries;

onMounted(async () => {
    workoutDiary.search();
});

const onAddEntry = function (): void {
    modal.show({
        component: WorkoutEntryModalComponent,
        componentProps: {},
    });
};
</script>

<style lang="scss">
@use '@/styling/variables' as *;

.workout-diary-view {
    .entry-list-container {
        width: 720px;
        max-width: 100%;
        margin: auto;
    }
}
</style>
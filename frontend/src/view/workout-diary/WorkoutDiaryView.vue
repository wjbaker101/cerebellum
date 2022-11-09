<template>
    <ViewComponent class="workout-diary-view flex flex-vertical gap" heading="Workout Diary">
        <template v-slot:header>
            <ButtonComponent class="primary">
                <IconComponent icon="plus" gap="right" />
                <span>New Entry</span>
            </ButtonComponent>
        </template>
        <div class="entry-list-container">
            <WorkoutEntryComponent v-for="entry in entries" :workoutEntry="entry" />
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import WorkoutEntryComponent from '@/view/workout-diary/component/WorkoutEntry.component.vue';
import dayjs from 'dayjs';

import { IWorkoutEntry } from './model/WorkoutEntry.model';

const entries: Array<IWorkoutEntry> = [
    {
        date: dayjs('2022-11-09'),
        startTime: dayjs('2022-11-09 15:00:00'),
        endTime: dayjs('2022-11-09 16:00:00'),
        weight: 78.0,
        exercises: [],
    },
    {
        date: dayjs('2022-11-08'),
        startTime: dayjs('2022-11-08 15:00:00'),
        endTime: dayjs('2022-11-08 16:00:00'),
        weight: 78.0,
        exercises: [],
    },
];
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
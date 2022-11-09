<template>
    <div class="workout-entry-component">
        <section class="flex gap align-items-center">
            <h3>{{ workoutEntry.date.format('ddd Do MMM') }}</h3>
            <div class="flex-auto">
                <ButtonComponent class="mini" @click="onEntryClick">
                    <IconComponent icon="menu" />
                </ButtonComponent>
            </div>
        </section>
        <section class="flex gap align-items-center">
            <p>
                <strong>Time: </strong>
                <span>{{ workoutEntry.startTime.format('ha') }}</span>
            </p>
            <p>
                <strong>Duration: </strong>
                <span>{{ workoutEntry.endTime?.diff(workoutEntry.startTime, 'minutes') }}m</span>
            </p>
            <p>
                <strong>Weight: </strong>
                <span>{{ workoutEntry.weight?.toFixed(1) }}kg</span>
            </p>
        </section>
        <section>
            <p>
                <strong>Decline DB: </strong>
                <span>2x20kg 10x17.5kg 8x22.5kg</span>
            </p>
            <p>
                <strong>Cable Lateral Raise: </strong>
                <span>10x6.8kg 10x6.8kg 10x6.8kg</span>
            </p>
        </section>
    </div>
</template>

<script setup lang="ts">
import WorkoutEntryModalComponent from '@/view/workout-diary/modal/WorkoutEntry.modal.component.vue';

import { useModal } from '@wjb/vue/use/modal.use';

import { IWorkoutEntry } from '@/view/workout-diary/model/WorkoutEntry.model';

const props = defineProps<{
    workoutEntry: IWorkoutEntry;
}>();

const modal = useModal();

const onEntryClick = function (): void {
    modal.show({
        component: WorkoutEntryModalComponent,
        componentProps: {
            workoutEntry: props.workoutEntry,
        },
    });
};
</script>

<style lang="scss">
@use '~@wjb/styling/variables' as *;

.workout-entry-component {
    padding: 0.5rem;

    @include shadow-small();

    & + .workout-entry-component {
        margin-top: 0.5rem;
    }

    & > section + section {
        border-top: 1px solid var(--wjb-secondary)
    }
}
</style>
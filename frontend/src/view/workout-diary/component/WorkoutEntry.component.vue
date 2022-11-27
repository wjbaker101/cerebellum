<template>
    <div class="workout-entry-component">
        <section>
            <div class="flex gap align-items-center">
                <h3>{{ workoutEntry.startAt.format('ddd Do MMM') }}</h3>
                <div class="flex-auto">
                    <ButtonComponent class="mini" @click="onEntryClick">
                        <IconComponent icon="menu" />
                    </ButtonComponent>
                </div>
            </div>
            <div class="flex gap align-items-center">
                <div>
                    <strong>Time: </strong>
                    <span>{{ workoutEntry.startAt.format('ha') }}</span>
                </div>
                <div>
                    <strong>Duration: </strong>
                    <span v-if="workoutEntry.endAt">{{ workoutEntry.endAt.diff(workoutEntry.startAt, 'minutes') }}m</span>
                    <span v-else>not finished</span>
                </div>
                <div>
                    <strong>Weight: </strong>
                    <span v-if="workoutEntry.weight">{{ workoutEntry.weight.toFixed(1) }}kg</span>
                    <span v-else>no data</span>
                </div>
            </div>
        </section>
        <section>
            <p v-if="workoutEntry.exercises.length === 0">No exercises added yet</p>
            <p v-else>
                <div v-for="exercise in workoutEntry.exercises">
                    <strong>{{ exercise.name }}: </strong>
                    <span v-for="set in exercise.sets">{{ set.repetitions }}x{{ set.weight }}kg </span>
                </div>
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
    border-top: 4px solid var(--wjb-secondary);
    border-radius: var(--wjb-border-radius);

    @include shadow-small();

    & + .workout-entry-component {
        margin-top: 0.5rem;
    }
}
</style>
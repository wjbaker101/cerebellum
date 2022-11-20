<template>
    <div class="workout-entry-modal-component">
        <h2>Edit List Items</h2>
        <FormComponent>
            <FormSectionComponent class="flex gap">
                <div>
                    <FormInputComponent label="Start">
                        <input type="datetime-local" v-model="form.date">
                    </FormInputComponent>
                </div>
                <div>
                    <FormInputComponent label="End">
                        <input type="time" placeholder="End Time" v-model="form.endTime">
                    </FormInputComponent>
                </div>
            </FormSectionComponent>
            <FormSectionComponent class="flex gap">
                <div>
                    <FormInputComponent label="Weight (kg)">
                        <input type="text" placeholder="Weight (kg)" v-model="form.weight">
                    </FormInputComponent>
                </div>
            </FormSectionComponent>
            <div class="flex gap align-items-center">
                <h3>Sets:</h3>
                <div class="flex-auto">
                    <ButtonComponent class="primary mini">
                        <IconComponent icon="plus" />
                    </ButtonComponent>
                </div>
            </div>
            <FormSectionComponent class="flex gap">
                <div>
                    <FormInputComponent label="Name">
                        <input type="text" placeholder="Name">
                    </FormInputComponent>
                </div>
                <div class="flex-auto">
                    <FormInputComponent label="Reps">
                        <input type="text" placeholder="Reps">
                    </FormInputComponent>
                </div>
                <div class="flex-auto">
                    <FormInputComponent>
                        &times;
                    </FormInputComponent>
                </div>
                <div class="flex-auto">
                    <FormInputComponent label="Weight (kg)">
                        <input type="text" placeholder="Weight (kg)">
                    </FormInputComponent>
                </div>
                <div class="flex-auto">
                    <FormInputComponent>
                        <ButtonComponent class="primary mini">
                            <IconComponent icon="plus" />
                        </ButtonComponent>
                    </FormInputComponent>
                </div>
            </FormSectionComponent>
            <FormSectionComponent>
                <FormInputComponent>
                    <ButtonComponent class="primary">
                        <IconComponent icon="tick" gap="right" />
                        <span>Confirm</span>
                    </ButtonComponent>
                </FormInputComponent>
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';

import { IWorkoutEntry } from '../model/WorkoutEntry.model';

const props = defineProps<{
    workoutEntry: IWorkoutEntry;
}>();

interface IWorkoutDiaryForm {
    date: string;
    startTime: string;
    endTime: string | null;
    weight: number | null;
    exercises: Array<{
        name: string;
        sets: Array<{
            repetitions: number;
            weight: number;
        }>;
    }>;
}

const form = reactive<IWorkoutDiaryForm>({
    date: props.workoutEntry.date.format('YYYY-MM-DDTHH:mm'),
    startTime: props.workoutEntry.startTime.format('HH:mm'),
    endTime: props.workoutEntry.endTime?.format('HH:mm') ?? null,
    weight: props.workoutEntry.weight,
    exercises: props.workoutEntry.exercises.map(exercise => ({
        name: exercise.name,
        sets: exercise.sets.map(set => ({
            repetitions: set.repetitions,
            weight: set.weight,
        })),
    })),
});
</script>

<style lang="scss">
.workout-entry-modal-component {
    width: 650px;
    max-width: 100%;
}
</style>
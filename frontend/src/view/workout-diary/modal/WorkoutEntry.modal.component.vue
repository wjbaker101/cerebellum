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
                    <FormInputComponent label="Body Weight (kg)">
                        <input type="text" placeholder="99.9" v-model="form.weight">
                    </FormInputComponent>
                </div>
            </FormSectionComponent>
            <div class="flex gap align-items-center">
                <h3>Exercises:</h3>
                <div class="flex-auto">
                    <ButtonComponent class="primary mini" @click="onAddExercise">
                        <IconComponent icon="plus" />
                    </ButtonComponent>
                </div>
            </div>
            <FormSectionComponent class="flex gap" v-for="exercise in form.exercises">
                <div>
                    <div class="flex gap-small align-items-center">
                        <div class="flex-auto">
                            <DeleteButtonComponent class="mini" only-icon />
                        </div>
                        <FormInputComponent label="Name">
                            <input type="text" placeholder="Name" v-model="exercise.name">
                        </FormInputComponent>
                    </div>
                </div>
                <div class="flex-auto">
                    <div class="flex gap-small align-items-center" v-for="(set, index) in exercise.sets">
                        <div class="flex-auto">
                            <FormInputComponent :label="index === 0 ? 'Reps' : ''">
                                <input class="set-input" type="text" placeholder="99" v-model="set.repetitions">
                            </FormInputComponent>
                        </div>
                        <div class="flex-auto">
                            <FormInputComponent>
                                &times;
                            </FormInputComponent>
                        </div>
                        <div class="flex-auto">
                            <FormInputComponent :label="index === 0 ? 'Weight (kg)' : ''">
                                <input class="set-input" type="text" placeholder="99.9" v-model="set.weight">
                            </FormInputComponent>
                        </div>
                        <div class="flex-auto" v-if="index === exercise.sets.length - 1">
                            <FormInputComponent>
                                <ButtonComponent class="primary mini" @click="onAddSet(exercise)">
                                    <IconComponent icon="plus" />
                                </ButtonComponent>
                            </FormInputComponent>
                        </div>
                    </div>
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
    exercises: Array<IFormWorkoutExercise>;
}

interface IFormWorkoutExercise {
    reference: string | null;
    name: string | null;
    sets: Array<IFormWorkoutSet>;
}

interface IFormWorkoutSet {
    reference: string | null;
    repetitions: number | null;
    weight: number | null;
}

const form = reactive<IWorkoutDiaryForm>({
    date: props.workoutEntry.date.format('YYYY-MM-DDTHH:mm'),
    startTime: props.workoutEntry.startTime.format('HH:mm'),
    endTime: props.workoutEntry.endTime?.format('HH:mm') ?? null,
    weight: props.workoutEntry.weight,
    exercises: props.workoutEntry.exercises.map(exercise => ({
        reference: exercise.reference,
        name: exercise.name,
        sets: exercise.sets.map(set => ({
            reference: set.reference,
            repetitions: set.repetitions,
            weight: set.weight,
        })),
    })),
});

const onAddExercise = function (): void {
    form.exercises.push({
        reference: null,
        name: null,
        sets: [
            {
                reference: null,
                repetitions: null,
                weight: null,
            },
        ],
    });
};

const onAddSet = function (exercise: IFormWorkoutExercise): void {
    exercise.sets.push({
        reference: null,
        repetitions: null,
        weight: null,
    });
};
</script>

<style lang="scss">
.workout-entry-modal-component {
    width: 650px;
    max-width: 100%;

    .set-input {
        width: 150px;
    }
}
</style>
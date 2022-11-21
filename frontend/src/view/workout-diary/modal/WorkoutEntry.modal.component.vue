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
            <FormSectionComponent class="flex gap" :key="`exercise-${exercise.createdAt.toISOString()}`" v-for="(exercise, exerciseIndex) in form.exercises">
                <div>
                    <div class="flex gap-small align-items-center">
                        <div class="flex-auto flex-align-self-end">
                            <DeleteButtonComponent v-if="exerciseIndex > 0" class="mini" only-icon @delete="onDeleteExercise(exerciseIndex)" />
                        </div>
                        <FormInputComponent label="Name">
                            <input type="text" placeholder="Name" v-model="exercise.name">
                        </FormInputComponent>
                    </div>
                </div>
                <div class="flex-auto">
                    <div class="flex gap-small align-items-center" :key="`set-${set.createdAt.toISOString()}`" v-for="(set, setIndex) in exercise.sets">
                        <div class="flex-auto">
                            <FormInputComponent :label="setIndex === 0 ? 'Reps' : ''">
                                <input class="set-input" type="text" placeholder="99" v-model="set.repetitions">
                            </FormInputComponent>
                        </div>
                        <div class="flex-auto">
                            <FormInputComponent>
                                &times;
                            </FormInputComponent>
                        </div>
                        <div class="flex-auto">
                            <FormInputComponent :label="setIndex === 0 ? 'Weight (kg)' : ''">
                                <input class="set-input" type="text" placeholder="99.9" v-model="set.weight">
                            </FormInputComponent>
                        </div>
                        <div class="flex-auto flex-align-self-end">
                            <ButtonComponent v-if="setIndex === 0" class="primary mini" @click="onAddSet(exercise)">
                                <IconComponent icon="plus" />
                            </ButtonComponent>
                            <DeleteButtonComponent v-else class="mini" only-icon @delete="onDeleteSet(exercise, setIndex)" />
                        </div>
                    </div>
                </div>
            </FormSectionComponent>
            <FormSectionComponent>
                <FormInputComponent>
                    <ButtonComponent class="primary" @click="onConfirm">
                        <IconComponent icon="tick" gap="right" />
                        <span>Confirm</span>
                    </ButtonComponent>
                </FormInputComponent>
                <div v-if="userMessageErrors.length > 0">
                    <p><strong>There were one or more errors preventing confirmation:</strong></p>
                    <ul>
                        <li v-for="error in userMessageErrors">{{ error }}</li>
                    </ul>
                </div>
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import dayjs, { Dayjs } from 'dayjs';

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
    createdAt: Dayjs;
    name: string;
    sets: Array<IFormWorkoutSet>;
}

interface IFormWorkoutSet {
    reference: string | null;
    createdAt: Dayjs;
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
        createdAt: dayjs(),
        name: exercise.name,
        sets: exercise.sets.map(set => ({
            reference: set.reference,
            createdAt: dayjs(),
            repetitions: set.repetitions,
            weight: set.weight,
        })),
    })),
});

const userMessageErrors = ref<Array<string>>([]);

const onAddExercise = function (): void {
    form.exercises.push({
        reference: null,
        createdAt: dayjs(),
        name: '',
        sets: [
            {
                reference: null,
                createdAt: dayjs(),
                repetitions: null,
                weight: null,
            },
        ],
    });
};

const onDeleteExercise = function (index: number): void {
    form.exercises.splice(index, 1);
};

const onAddSet = function (exercise: IFormWorkoutExercise): void {
    exercise.sets.push({
        reference: null,
        createdAt: dayjs(),
        repetitions: null,
        weight: null,
    });
};

const onDeleteSet = function (exercise: IFormWorkoutExercise, index: number): void {
    exercise.sets.splice(index, 1);
};

const onConfirm = function (): void {
    const errors = validate();

    if (errors.length > 0) {
        userMessageErrors.value = errors;
        return;
    }
};

const validate = function(): Array<string> {
    const errors: Array<string> = [];

    if (form.date.length === 0)
        errors.push('Invalid start time');

    if (form.exercises.some(x => x.name.length === 0))
        errors.push('Exercise(s) with an invalid name');

    if (form.exercises.flatMap(x => x.sets).some(x => x.repetitions === null))
        errors.push('Set(s) with an invalid number of repetitions');

    if (form.exercises.flatMap(x => x.sets).some(x => x.weight === null))
        errors.push('Set(s) with an invalid weight');

    return errors;
};
</script>

<style lang="scss">
.workout-entry-modal-component {
    width: 650px;
    max-width: 100%;

    .set-input {
        width: 150px;
    }

    .flex-align-self-end {
        align-self: flex-end;
    }
}
</style>
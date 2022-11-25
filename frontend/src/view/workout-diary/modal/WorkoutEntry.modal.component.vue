<template>
    <div class="workout-entry-modal-component">
        <h2>{{ workoutEntry ? 'Edit' : 'Create' }} Workout Entry</h2>
        <FormComponent>
            <FormSectionComponent class="flex gap breakpoint">
                <FormInputComponent label="Start">
                    <input type="datetime-local" v-model="form.startTime">
                </FormInputComponent>
            </FormSectionComponent>
            <FormSectionComponent class="flex gap align-items-end">
                <div class="flex gap-small align-items-end">
                    <div>
                        <FormInputComponent label="End">
                            <input type="time" placeholder="End Time" v-model="form.endTime">
                        </FormInputComponent>
                    </div>
                    <div class="flex-auto">
                        <ButtonComponent class="mini" @click="onSetEndTime">
                            <IconComponent icon="clock" />
                        </ButtonComponent>
                    </div>
                </div>
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
            <FormSectionComponent class="flex gap breakpoint" :key="`exercise-${exercise.createdAt.toISOString()}`" v-for="(exercise, exerciseIndex) in form.exercises">
                <div class="flex-1">
                    <div class="flex gap-small align-items-end">
                        <div v-if="exerciseIndex > 0" class="flex-auto">
                            <DeleteButtonComponent class="mini" only-icon @delete="onDeleteExercise(exerciseIndex)" />
                        </div>
                        <FormInputComponent label="Name">
                            <input type="text" placeholder="Name" v-model="exercise.name">
                        </FormInputComponent>
                    </div>
                </div>
                <div class="flex-1">
                    <div class="flex gap-small align-items-end" :key="`set-${set.createdAt.toISOString()}`" v-for="(set, setIndex) in exercise.sets">
                        <div class="flex-1">
                            <FormInputComponent :label="setIndex === 0 ? 'Reps' : ''">
                                <input type="text" placeholder="99" v-model="set.repetitions">
                            </FormInputComponent>
                        </div>
                        <div class="flex-auto">
                            <FormInputComponent>
                                &times;
                            </FormInputComponent>
                        </div>
                        <div class="flex-1">
                            <FormInputComponent :label="setIndex === 0 ? 'Weight (kg)' : ''">
                                <input type="text" placeholder="99.9" v-model="set.weight">
                            </FormInputComponent>
                        </div>
                        <div class="flex-auto">
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
            <FormSectionComponent>
                <FormInputComponent>
                    <DeleteButtonComponent @delete="onDelete" />
                </FormInputComponent>
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import dayjs, { Dayjs } from 'dayjs';

import { useModal } from '@wjb/vue/use/modal.use';
import { usePopup } from '@wjb/vue/use/popup.use';
import { useApi } from '@/use/api/api.use';

import { helper } from '@/view/workout-diary/helper/helper';
import { useWorkoutDiary } from '@/view/workout-diary/use/workout-diary.use';

import { IWorkoutEntry } from '@/view/workout-diary/model/WorkoutEntry.model';

const props = defineProps<{
    workoutEntry?: IWorkoutEntry;
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

const api = useApi();
const modal = useModal();
const popup = usePopup();
const workoutDiary = useWorkoutDiary();

const form = reactive<IWorkoutDiaryForm>({
    date: (props.workoutEntry?.date ?? helper.roundDayjs(dayjs(), 5)).format('YYYY-MM-DDTHH:mm'),
    startTime: (props.workoutEntry?.startTime ?? helper.roundDayjs(dayjs(), 5)).format('YYYY-MM-DDTHH:mm'),
    endTime: props.workoutEntry?.endTime?.format('HH:mm') ?? null,
    weight: props.workoutEntry?.weight ?? null,
    exercises: props.workoutEntry?.exercises.map(exercise => ({
        reference: exercise.reference,
        createdAt: dayjs(),
        name: exercise.name,
        sets: exercise.sets.map(set => ({
            reference: set.reference,
            createdAt: dayjs(),
            repetitions: set.repetitions,
            weight: set.weight,
        })),
    })) ?? [],
});

const userMessageErrors = ref<Array<string>>([]);

const onSetEndTime = function (): void {
    form.endTime = helper.roundDayjs(dayjs(), 5).format('HH:mm');
};

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

const onConfirm = async function (): Promise<void> {
    const startTime = dayjs(form.startTime);
    const endTime = form.endTime === null || form.endTime.length === 0 ? null : timeToDayjs(startTime, form.endTime);

    userMessageErrors.value = [];

    const errors = validate({
        startTime,
        endTime,
    });
    if (errors.length > 0) {
        userMessageErrors.value = errors;
        return;
    }

    if (props.workoutEntry) {
        await workoutDiary.updateEntry(props.workoutEntry.reference, {
            date: startTime.toISOString(),
            startTime: startTime.toISOString(),
            endTime: endTime?.toISOString() ?? null,
            weight: form.weight,
            exercises: form.exercises.map(exercise => ({
                reference: exercise.reference,
                name: exercise.name,
                sets: exercise.sets.map(set => ({
                    reference: set.reference,
                    repetitions: set.repetitions ?? 0,
                    weight: set.weight ?? 0,
                })),
            })),
        });

        popup.trigger({
            style: 'success',
            message: 'Entry has been updated',
        });
    }
    else {
        await workoutDiary.createEntry({
            date: startTime.toISOString(),
            startTime: startTime.toISOString(),
            endTime: endTime?.toISOString() ?? null,
            weight: form.weight,
            exercises: form.exercises.map(exercise => ({
                reference: exercise.reference,
                name: exercise.name,
                sets: exercise.sets.map(set => ({
                    reference: set.reference,
                    repetitions: set.repetitions ?? 0,
                    weight: set.weight ?? 0,
                })),
            })),
        });

        popup.trigger({
            style: 'success',
            message: 'Entry has been created',
        });
    }
};

const timeToDayjs = function (baseDayjs: Dayjs, time: string): Dayjs {
    const split = time.split(':');

    return baseDayjs.hour(Number(split[0])).minute(Number(split[1]));
};

const validate = function(values: {
    startTime: Dayjs;
    endTime: Dayjs | null;
}): Array<string> {
    const errors: Array<string> = [];

    if (form.date.length === 0)
        errors.push('Invalid start time');

    if (values.endTime !== null && values.startTime.isAfter(values.endTime))
        errors.push('Start time is after end time');

    if (form.exercises.some(x => x.name.length === 0))
        errors.push('Exercise(s) with an invalid name');

    if (form.exercises.flatMap(x => x.sets).some(x => x.repetitions === null))
        errors.push('Set(s) with an invalid number of repetitions');

    if (form.exercises.flatMap(x => x.sets).some(x => x.weight === null))
        errors.push('Set(s) with an invalid weight');

    return errors;
};

const onDelete = async function (): Promise<void> {
    if (!props.workoutEntry)
        return;

    await workoutDiary.deleteEntry(props.workoutEntry.reference);

    modal.hide();

    popup.trigger({
        style: 'success',
        message: 'Entry has been deleted',
    });
};
</script>

<style lang="scss">
.workout-entry-modal-component {
    width: 650px;
    max-width: 100%;
}
</style>
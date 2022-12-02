<template>
    <div class="workout-entry-modal-component">
        <h2>{{ workoutEntry ? 'Edit' : 'Create' }} Workout Entry</h2>
        <FormComponent>
            <FormSectionComponent class="flex gap breakpoint">
                <FormInputComponent label="Start">
                    <input type="datetime-local" v-model="form.startAt">
                </FormInputComponent>
            </FormSectionComponent>
            <FormSectionComponent class="flex gap align-items-end">
                <div class="flex gap-small align-items-end">
                    <div>
                        <FormInputComponent label="End">
                            <input type="time" placeholder="End Time" v-model="form.endAt">
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
                        <input type="number" placeholder="99.9" v-model="form.weight" @keypress.enter="onConfirm">
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
                            <input :ref="refOfExercise(exerciseIndex, 'name')" type="text" placeholder="Name" v-model="exercise.name" @keypress.enter="goTo(refOfSet(exerciseIndex, 0, 'repetitions'))">
                        </FormInputComponent>
                    </div>
                </div>
                <div class="flex-1">
                    <div class="flex gap-small align-items-end" :key="`set-${set.createdAt.toISOString()}`" v-for="(set, setIndex) in exercise.sets">
                        <div class="flex-1">
                            <FormInputComponent :label="setIndex === 0 ? 'Reps' : ''">
                                <input :ref="refOfSet(exerciseIndex, setIndex, 'repetitions')" type="number" placeholder="99" v-model="set.repetitions" @keypress.enter="goTo(refOfSet(exerciseIndex, setIndex, 'weight'))">
                            </FormInputComponent>
                        </div>
                        <div class="flex-auto">
                            &times;
                        </div>
                        <div class="flex-1">
                            <FormInputComponent :label="setIndex === 0 ? 'Weight (kg)' : ''">
                                <input :ref="refOfSet(exerciseIndex, setIndex, 'weight')" type="number" placeholder="99.9" v-model="set.weight">
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
                <div>
                    <ButtonComponent class="primary" @click="onConfirm">
                        <IconComponent icon="tick" gap="right" />
                        <span>Confirm</span>
                    </ButtonComponent>
                </div>
                <div v-if="userMessageErrors.length > 0">
                    <p><strong>There were one or more errors preventing confirmation:</strong></p>
                    <ul>
                        <li v-for="error in userMessageErrors">{{ error }}</li>
                    </ul>
                </div>
            </FormSectionComponent>
            <FormSectionComponent>
                <DeleteButtonComponent @delete="onDelete" />
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { getCurrentInstance, ref } from 'vue';
import dayjs, { Dayjs } from 'dayjs';

import { useModal } from '@wjb/vue/use/modal.use';
import { usePopup } from '@wjb/vue/use/popup.use';

import { helper } from '@/view/workout-diary/helper/helper';
import { useWorkoutDiary } from '@/view/workout-diary/use/workout-diary.use';

import { IWorkoutEntry } from '@/view/workout-diary/model/WorkoutEntry.model';

const props = defineProps<{
    workoutEntry?: IWorkoutEntry;
}>();

const instance = getCurrentInstance();

interface IWorkoutDiaryForm {
    reference: string | null;
    startAt: string;
    endAt: string | null;
    weight: string | null;
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
    repetitions: string | null;
    weight: string | null;
}

const modal = useModal();
const popup = usePopup();
const workoutDiary = useWorkoutDiary();

const mapWorkoutEntry = function (workoutEntry?: IWorkoutEntry): IWorkoutDiaryForm {
    return {
        reference: workoutEntry?.reference ?? null,
        startAt: (workoutEntry?.startAt ?? helper.roundDayjs(dayjs(), 5)).format('YYYY-MM-DDTHH:mm'),
        endAt: workoutEntry?.endAt?.format('HH:mm') ?? null,
        weight: workoutEntry?.weight?.toFixed(1) ?? null,
        exercises: workoutEntry?.exercises.map(exercise => ({
            reference: exercise.reference,
            createdAt: dayjs(),
            name: exercise.name,
            sets: exercise.sets.map(set => ({
                reference: set.reference,
                createdAt: dayjs(),
                repetitions: String(set.repetitions),
                weight: set.weight.toFixed(1),
            })),
        })) ?? [],
    }
};

const form = ref<IWorkoutDiaryForm>(mapWorkoutEntry(props.workoutEntry));

const userMessageErrors = ref<Array<string>>([]);

const refOfExercise = function (index: number, element: string): string {
    return `exercise-${index}/${element}`;
};

const refOfSet = function (exerciseIndex: number, setIndex: number, element: string): string {
    return `exercise-${exerciseIndex}/set-${setIndex}/${element}`;
};

const goTo = function (ref: string): void {
    const element = instance?.refs[ref] as HTMLElement[] | undefined;
    if (!element)
        return;

    element[0].focus();
};

const onSetEndTime = async function (): Promise<void> {
    form.value.endAt = helper.roundDayjs(dayjs(), 5).format('HH:mm');

    await onConfirm();
};

const onAddExercise = function (): void {
    form.value.exercises.push({
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
    form.value.exercises.splice(index, 1);
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
    const startAt = dayjs(form.value.startAt);
    const endAt = form.value.endAt === null || form.value.endAt.length === 0 ? null : timeToDayjs(startAt, form.value.endAt);

    userMessageErrors.value = [];

    const errors = validate({
        startAt: startAt,
        endAt: endAt,
    });
    if (errors.length > 0) {
        userMessageErrors.value = errors;
        return;
    }

    if (form.value.reference !== null) {
        const result = await workoutDiary.updateEntry(form.value.reference, {
            startAt: startAt.toISOString(),
            endAt: endAt?.toISOString() ?? null,
            weight: form.value.weight ? Number(form.value.weight) : null,
            exercises: form.value.exercises.map(exercise => ({
                reference: exercise.reference,
                name: exercise.name,
                sets: exercise.sets.map(set => ({
                    reference: set.reference,
                    repetitions: Number(set.repetitions ?? '0'),
                    weight: Number(set.weight ?? '0'),
                })),
            })),
        });

        form.value = mapWorkoutEntry(result);

        popup.trigger({
            style: 'success',
            message: 'Entry has been updated',
        });
    }
    else {
        const result = await workoutDiary.createEntry({
            startAt: startAt.toISOString(),
            endAt: endAt?.toISOString() ?? null,
            weight: form.value.weight ? Number(form.value.weight) : null,
            exercises: form.value.exercises.map(exercise => ({
                reference: exercise.reference,
                name: exercise.name,
                sets: exercise.sets.map(set => ({
                    reference: set.reference,
                    repetitions: Number(set.repetitions ?? '0'),
                    weight: Number(set.weight ?? '0'),
                })),
            })),
        });

        form.value = mapWorkoutEntry(result);

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
    startAt: Dayjs;
    endAt: Dayjs | null;
}): Array<string> {
    const errors: Array<string> = [];

    if (form.value.startAt.length === 0)
        errors.push('Invalid start time');

    if (helper.isNaN(form.value.weight ?? '0'))
        errors.push('Invalid body weight');

    if (values.endAt !== null && values.startAt.isAfter(values.endAt))
        errors.push('Start time is after end time');

    if (form.value.exercises.some(x => x.name.length === 0))
        errors.push('Exercise(s) with an invalid name');

    if (form.value.exercises.flatMap(x => x.sets).some(x => x.repetitions === null || helper.isNaN(x.repetitions)))
        errors.push('Set(s) with an invalid number of repetitions');

    if (form.value.exercises.flatMap(x => x.sets).some(x => x.weight === null || helper.isNaN(x.weight)))
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
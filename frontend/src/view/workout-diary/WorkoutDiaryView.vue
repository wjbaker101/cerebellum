<template>
    <ViewComponent class="flex flex-vertical workout-diary-view gap" heading="Workout Diary">
        <template v-slot:header>
            <ButtonComponent class="primary" @click="onAddEntry">
                <IconComponent icon="plus" gap="right" />
                <span>New Entry</span>
            </ButtonComponent>
        </template>
        <ListComponent :items="entries" itemName="entries" removeStyling>
            <template #item="{ item }">
                <WorkoutEntryComponent :workoutEntry="item" @showModal="onShowModal" />
            </template>
        </ListComponent>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useRoute } from 'vue-router';

import ListComponent from '@/component/ListComponent.vue';
import WorkoutEntryComponent from '@/view/workout-diary/component/WorkoutEntry.component.vue';
import WorkoutEntryModalComponent, { IWorkoutEntryModalProps } from '@/view/workout-diary/modal/WorkoutEntry.modal.component.vue';

import { useModal } from '@/composables/modal.use';
import { useWorkoutDiary } from '@/view/workout-diary/use/workout-diary.use';

import { IWorkoutEntry } from './model/WorkoutEntry.model';
import ViewComponent from '@/component/ViewComponent.vue';
import ButtonComponent from '@/component/ButtonComponent.vue';
import IconComponent from '@/component/IconComponent.vue';

const route = useRoute();
const modal = useModal();
const workoutDiary = useWorkoutDiary();

const entryReference = route.params.entryReference as string;

const entries = workoutDiary.entries;

const onShowModal = function (entry: IWorkoutEntry): void {
    modal.show<IWorkoutEntryModalProps>({
        component: WorkoutEntryModalComponent,
        componentProps: {
            workoutEntry: entry,
        },
        onClose() {
            history.pushState({}, '', '/workout-diary');
        },
    });
};

onMounted(async () => {
    await workoutDiary.search();

    if (entryReference) {
        const entry = entries.value?.find(x => x.reference === entryReference);
        if (entry)
            onShowModal(entry);
    }
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
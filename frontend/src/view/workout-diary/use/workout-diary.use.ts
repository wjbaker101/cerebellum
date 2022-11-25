import { ref } from 'vue';

import { useApi } from '@/use/api/api.use';

import { IWorkoutEntry } from '@/view/workout-diary/model/WorkoutEntry.model';
import { ICreateWorkoutDiaryEntryRequest } from '@/use/api/type/CreateWorkoutDiaryEntry.type';
import { IUpdateWorkoutDiaryEntryRequest } from '@/use/api/type/UpdateWorkoutDiaryEntry.type';

const api = useApi();

const entries = ref<Array<IWorkoutEntry> | null>(null);

export const useWorkoutDiary = function () {
    return {
        entries: entries,

        async search(): Promise<void> {
            const result = await api.workoutDiary.search();

            entries.value = result;
        },

        async createEntry(request: ICreateWorkoutDiaryEntryRequest): Promise<void> {
            const result = await api.workoutDiary.createEntry(request);

            entries.value?.push(result);
        },

        async updateEntry(reference: string, request: IUpdateWorkoutDiaryEntryRequest): Promise<void> {
            const result = await api.workoutDiary.updateEntry(reference, request);

            const entry = entries.value?.find(x => x.reference === reference);
            if (entry) {
                entry.date = result.date;
                entry.startTime = result.startTime;
                entry.endTime = result.endTime;
                entry.weight = result.weight;
                entry.exercises = result.exercises;
            }
        },

        async deleteEntry(reference: string): Promise<void> {
            await api.workoutDiary.deleteEntry(reference);

            const index = entries.value?.findIndex(x => x.reference === reference);
            if (index)
                entries.value?.splice(index, 1);
        },
    };
};
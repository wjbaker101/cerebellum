import { readonly, ref } from 'vue';
import { Dayjs } from 'dayjs';

import { useApi } from '@/api/api.use';

import { ICalendarEntry } from '@/models/CalendarEntry.model';

const api = useApi();

const entries = ref<Array<ICalendarEntry> | null>([]);

export const useCalendar = function () {
    return {
        entries: readonly(entries),

        async searchEntries(startAt: Dayjs, endAt: Dayjs): Promise<void> {
            const result = await api.calendar.searchEntries(startAt, endAt);

            entries.value = result;
        },

        add(entry: ICalendarEntry): void {
            entries.value?.push(entry);
        },

        remove(reference: string): void {
            entries.value = entries.value?.filter(x => x.reference !== reference) ?? null;
        },
    };
};
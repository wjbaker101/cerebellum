import { readonly, ref } from 'vue';

import { ICalendarEntry } from '@/model/CalendarEntry.model';

const entries = ref<Array<ICalendarEntry> | null>([]);

export const useCalendar = function () {
    return {
        entries: readonly(entries),

        add(entry: ICalendarEntry): void {
            entries.value?.push(entry);
        },

        remove(reference: string): void {
            entries.value = entries.value?.filter(x => x.reference !== reference) ?? null;
        },
    };
};
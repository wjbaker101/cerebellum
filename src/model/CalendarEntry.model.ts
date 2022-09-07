import { Dayjs } from 'dayjs';

export interface ICalendarEntry {
    readonly reference: string;
    readonly createdAt: Dayjs;
    description: string;
    startAt: Dayjs;
    endAt: Dayjs;
    recurringPeriod: CalendarRecurringPeriod;
}

export type CalendarRecurringPeriod = 'none' | 'weekly' | 'monthly' | 'yearly';
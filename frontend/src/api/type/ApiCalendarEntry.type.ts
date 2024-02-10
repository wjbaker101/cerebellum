import { Dayjs } from 'dayjs';

export interface IApiCalendarEntry {
    readonly reference: string;
    readonly createdAt: Dayjs;
    readonly description: string;
    readonly startAt: Dayjs;
    readonly endAt: Dayjs;
    readonly recurringPeriod: number;
};
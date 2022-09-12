import { IApiCalendarEntry } from '@/use/api/type/ApiCalendarEntry.type';

export interface IAddCalendarEntryRequest {
    description: string;
    startAt: string;
    endAt: string;
    recurringPeriod: number;
};

export interface IAddCalendarEntryResponse {
    calendarEntry: IApiCalendarEntry;
};
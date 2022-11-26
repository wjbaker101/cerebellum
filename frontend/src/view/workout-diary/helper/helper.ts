import { Dayjs } from 'dayjs';

export const helper = {

    roundDayjs(dayjs: Dayjs, roundBy: number): Dayjs {
        const newMinutes = Math.round(dayjs.minute() / roundBy) * roundBy;
        return dayjs.minute(newMinutes);
    },

    isNaN(value: string): boolean {
        return Number.isNaN(Number(value));
    },

};
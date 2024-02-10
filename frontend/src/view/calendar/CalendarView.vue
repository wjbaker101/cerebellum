<template>
    <ViewComponent class="calendar-view flex flex-vertical gap-small" heading="Calendar" no-header>
        <div class="calendar flex flex-vertical">
            <div class="header flex flex-auto align-items-center gap-small text-centered">
                <ButtonComponent @click="incrementMonth(-1)" class="primary">
                    <IconComponent icon="arrow-left"></IconComponent>
                </ButtonComponent>
                <div class="flex gap-small">
                    <ButtonComponent @click="resetMonth" class="primary">
                        <span>{{ date.format('MMMM') }}</span>
                        <span v-if="date.year() !== now.year()">&nbsp;{{ date.year() }}</span>
                    </ButtonComponent>
                    <ButtonComponent @click="onNew" class="secondary flex-auto">
                        <IconComponent icon="plus"></IconComponent>
                    </ButtonComponent>
                </div>
                <ButtonComponent @click="incrementMonth(1)" class="primary">
                    <IconComponent icon="arrow-right"></IconComponent>
                </ButtonComponent>
            </div>
            <div class="flex flex-auto text-centered">
                <div>Mon</div>
                <div>Tues</div>
                <div>Weds</div>
                <div>Thurs</div>
                <div>Fri</div>
                <div>Sat</div>
                <div>Sun</div>
            </div>
            <div class="days">
                <GradientBorderComponent
                    class="day"
                    :class="{
                        'is-today': day.isToday(),
                        'is-different-month': date.month() !== day.month()
                    }"
                    v-for="day in days"
                    :disabled="!day.isToday()"
                >
                    <div class="day-header">{{ day.date() }}</div>
                    <div>
                        <div
                            v-for="entry in entries?.filter(x => shouldShowEntryForDay(x, day))"
                            class="entry"
                            :class="{ 'is-past': entry.startAt.isBefore(now) }"
                        >
                            <span class="time">{{ entry.startAt.format('HH:MM') }}: </span>
                            <span class="description">{{ entry.description }}</span>
                        </div>
                    </div>
                </GradientBorderComponent>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref, computed, watchEffect } from 'vue';
import dayjs, { Dayjs } from 'dayjs';

import { useCalendar } from '@/use/calendar.use';
import { useModal } from '@wjb/vue/use/modal.use';

import NewEntryModalComponent from '@/view/calendar/modal/NewEntryComponent.modal.vue';
import GradientBorderComponent from '@/component/GradientBorderComponent.vue';

import { ICalendarEntry } from '@/models/CalendarEntry.model.js';

const calendar = useCalendar();
const modal = useModal();

const entries = calendar.entries;

const now = dayjs();
const date = ref<Dayjs>(dayjs().startOf('day'));

const incrementMonth = function (increment: number): void {
    date.value = date.value.add(increment, 'months');
};

const resetMonth = function (): void {
    date.value = dayjs();
};

const days = computed<Array<Dayjs>>(() => {
    const dayOfWeek = (date.value.startOf('month').day() + 6) % 7;
    const daysTilLastOfMonth = date.value.daysInMonth() + dayOfWeek;
    const remainingDays = 7 - (daysTilLastOfMonth % 7);

    const placeholder = Array(daysTilLastOfMonth + remainingDays).fill(0);
    return placeholder.map((_, index) => date.value.date(index + 1 - dayOfWeek));
});

const isDateWithinBounds = function (day: Dayjs, start: Dayjs, end: Dayjs) {
    return day.isSameOrAfter(start.startOf('day')) && day.isSameOrBefore(end.endOf('day'));
};

const shouldShowEntryForDay = function (entry: ICalendarEntry, day: Dayjs) {
    if (day.isBefore(entry.startAt.startOf('day')))
        return false;

    if (isDateWithinBounds(day, entry.startAt, entry.endAt))
        return true;

    if (entry.recurringPeriod === 'weekly') {
        const startDay = entry.startAt.day();
        const endDay = entry.endAt.day();

        if (day.day() >= startDay && day.day() <= endDay) {
            return true;
        }
    }

    if (entry.recurringPeriod === 'monthly') {
        const transformedStartAt = entry.startAt.year(day.year()).month(day.month());
        const transformedEndAt = entry.endAt.year(day.year()).month(day.month());

        if (isDateWithinBounds(day, transformedStartAt, transformedEndAt))
            return true;
    }

    if (entry.recurringPeriod === 'yearly') {
        const transformedStartAt = entry.startAt.year(day.year());
        const transformedEndAt = entry.endAt.year(day.year());

        if (isDateWithinBounds(day, transformedStartAt, transformedEndAt))
            return true;
    }
};

const onNew = function() {
    modal.show({
        style: 'centered',
        component: NewEntryModalComponent,
        componentProps: {},
    });
};

watchEffect(async () => {
    await calendar.searchEntries(days.value[0].startOf('day'), days.value[days.value.length - 1].endOf('day'));
});
</script>

<style lang="scss">
@use '@/styling/variables' as *;

.calendar-view {

    .calendar {
        .header {
            margin-bottom: 0.5rem;
        }

        .days {
            border-radius: var(--wjb-border-radius);
            display: grid;
            grid-template-columns: repeat(7, 1fr);
            grid-auto-rows: 1fr;
            gap: 0.125rem;

            .day {
                padding: 0.5rem;

                @include shadow-small();

                &.is-different-month {
                    opacity: 0.2;
                }

                &.is-today {
                    .day-header {
                        font-weight: bold;
                    }
                }

                .entry {
                    &.is-past .description {
                        text-decoration: line-through;
                    }
                }
            }
        }
    }
}
</style>@/models/CalendarEntry.model.js
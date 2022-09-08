<template>
    <div class="new-entry-modal-component">
        <h2>New Entry...</h2>
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Description">
                    <input type="text" placeholder="Eat lunch" v-model="description">
                </FormInputComponent>
                <FormInputComponent label="Start & End">
                    <DatePicker datetime range dark v-model="dateRange" weekStart="1" />
                </FormInputComponent>
                <FormInputComponent label="Recurring Period">
                    <select v-model="recurringPeriod">
                        <option v-for="(item, key) in recurringPeriods" :value="key">{{ item }}</option>
                    </select>
                </FormInputComponent>
            </FormSectionComponent>
            <FormSectionComponent>
                <ButtonComponent class="primary" @click="onCreate">Create</ButtonComponent>
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import dayjs from 'dayjs';
import DatePicker from '@vuepic/vue-datepicker';

import { useCalendar } from '@/use/calendar.use';

import { CalendarRecurringPeriod } from '@/model/CalendarEntry.model';

const calendar = useCalendar();

const recurringPeriods: Record<CalendarRecurringPeriod, string> = {
    none: 'None',
    weekly: 'Every Week',
    monthly: 'Every Month',
    yearly: 'Every Year',
};

const description = ref<string>('');
const recurringPeriod = ref<CalendarRecurringPeriod>('none');

const startAt = dayjs().add(1, 'hour').startOf('hour');
const dateRange = ref<[Date, Date]>([startAt.toDate(), startAt.add(1, 'hour').toDate()]);

const onCreate = function () {
    calendar.add({
        reference: 'ref',
        createdAt: dayjs(),
        description: description.value,
        recurringPeriod: recurringPeriod.value,
        startAt: dayjs(dateRange.value[0]),
        endAt: dayjs(dateRange.value[1]),
    });
};
</script>

<style lang="scss">
@import '~@vuepic/vue-datepicker/dist/main.css';

.new-entry-modal-component {
    width: 520px;
}
</style>
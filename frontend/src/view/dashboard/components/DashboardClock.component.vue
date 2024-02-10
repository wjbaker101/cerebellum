<template>
    <div class="dashboard-clock-component">
        <div class="time">{{ now.format('HH:mm') }}<small class="seconds">{{ now.format(':ss') }}</small></div>
        <div class="date flex align-items-center gap-small">
            <div class="flex-auto">
                <img width="32" height="32" v-if="weather !== null" :src="`/static/icon/weather/${weather.icon}.svg`">
            </div>
            <div class="flex-auto">
                {{ now.format('dddd Do YYYY') }}
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import dayjs, { Dayjs } from 'dayjs';

import { openWeatherApi } from '@/api/open-weather/open-weather.api';

import { IWeather } from '@/models/Weather.model';

const now = ref<Dayjs>(dayjs());
const weather = ref<IWeather | null>(null);

const updateTime = function () {
    setTimeout(() => {
        now.value = dayjs();
        updateTime();
    }, 1000);
};

onMounted(async () => {
    updateTime();
    weather.value = await openWeatherApi.getWeather('Maidstone,UK');
});
</script>

<style lang="scss">
@use '@/styling/variables' as *;

.dashboard-clock-component {
    margin: auto;

    .time {
        font-size: 10rem;
    }

    .seconds {
        padding-left: 0.25rem;
        font-size: 2rem;
        vertical-align: baseline;
    }

    .date {
        justify-content: center;
        margin-top: 2rem;
        font-size: 1.25rem;
        color: #aaa;
    }
}

@media screen and (max-width: $breakpoint) {
    .dashboard-clock-component {
        .time {
            font-size: 6rem;
        }
    }
}
</style>@/models/Weather.model
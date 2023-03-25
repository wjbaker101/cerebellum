<template>
    <ViewComponent class="dashboard-view flex flex-vertical" heading="Dashboard" no-header>
        <div class="flex-2 flex text-centered">
            <div class="clock">
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
        </div>
        <div v-if="dashboard">
            <div class="content-width">
                <div class="dashboard-items">
                    <RouterLink class="border text-centered" :key="item.reference" v-for="item in dashboard.items" :to="mapLink(item)">
                        <GradientBorderComponent class="item flex">
                            <div class="flex-auto">
                                <IconComponent v-if="item.type === 'note'" icon="page-text" size="large" />
                                <IconComponent v-else-if="item.type === 'list'" icon="menu" size="large" />
                                <IconComponent v-else-if="item.type === 'kanban-board'" icon="menu" size="large" style="transform: rotate(90deg)" />
                                <div class="title">{{ item.title }}</div>
                            </div>
                        </GradientBorderComponent>
                    </RouterLink>
                </div>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import dayjs, { Dayjs } from 'dayjs';

import GradientBorderComponent from '@/component/GradientBorderComponent.vue';

import { openWeatherApi } from '@/api/open-weather/open-weather.api';
import { useApi } from '@/use/api/api.use';

import { IWeather } from '@/model/Weather.model';
import { IDashboard, IDashboardItem } from '@/model/Dashboard.model';

const api = useApi();

const now = ref<Dayjs>(dayjs());
const weather = ref<IWeather | null>(null);

const dashboard = ref<IDashboard | null>(null);

const updateTime = function () {
    setTimeout(() => {
        now.value = dayjs();
        updateTime();
    }, 1000);
};

const mapLink = function (item: IDashboardItem): string {
    switch (item.type) {
        case 'note':
            return `/notes/${item.reference}`;
        case 'list':
            return `/lists/${item.reference}`;
        case 'kanban-board':
            return `/kanban/${item.reference}`;
        default:
            return '/';
    }
};

onMounted(async () => {
    updateTime();

    weather.value = await openWeatherApi.getWeather('Maidstone,UK');

    dashboard.value = await api.dashboard.get();

});
</script>

<style lang="scss">
@use '@/styling/variables.scss' as *;

.dashboard-view {

    .clock {
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
        .clock {
            .time {
                font-size: 6rem;
            }
        }
    }

    .dashboard-items {
        display: grid;
        gap: 2rem;
        grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));

        & > * {
            display: block;
            color: inherit;
            text-decoration: inherit;
        }

        .item {
            position: relative;
            padding: 0.5rem;
            border-radius: var(--wjb-border-radius);
            aspect-ratio: 1 / 1;
            background-color: var(--wjb-background-colour);

            @include shadow-small();

            &:hover {
                transform: scale(1.2);
                z-index: 1;
            }

            & > * {
                margin: auto;
            }

            .title {
                margin-top: 0.5rem;
            }
        }
    }
}
</style>
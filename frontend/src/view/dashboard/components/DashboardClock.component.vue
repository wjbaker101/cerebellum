<template>
    <div class="dashboard-clock-component">
        <div class="time">{{ now.format('HH:mm') }}<small class="seconds">{{ now.format(':ss') }}</small></div>
        <div class="date">
            {{ now.format('dddd Do YYYY') }}
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import dayjs, { Dayjs } from 'dayjs';

const now = ref<Dayjs>(dayjs());

const updateTime = function () {
    setTimeout(() => {
        now.value = dayjs();
        updateTime();
    }, 1000);
};

onMounted(async () => {
    updateTime();
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
</style>
<template>
    <div class="nav-clock-component" :class="{ 'is-enabled': enabled !== false }">
        <small>{{ now.format('HH:mm:ss') }}</small>
    </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import dayjs, { Dayjs } from 'dayjs';

const props = defineProps<{
    enabled?: boolean;
}>();

const now = ref<Dayjs>(dayjs());

const updateTime = function () {
    if (props.enabled === false)
        return;

    setTimeout(() => {
        now.value = dayjs();

        if (props.enabled === false)
            return;

        updateTime();
    }, 1000);
};

updateTime();

watch(() => props.enabled, (value, prev) => {
    if (prev === false && value === true)
        updateTime();
});
</script>

<style lang="scss">
.nav-clock-component {
    font-size: 1rem;
    font-weight: normal;
    clip-path: inset(0 50% 0 50% round var(--wjb-border-radius));
    opacity: 0.5;

    &.is-enabled {
        clip-path: inset(0 0 0 0 round 0);
        opacity: 1;
    }
}
</style>
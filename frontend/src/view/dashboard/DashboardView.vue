<template>
    <ViewComponent class="dashboard-view flex flex-vertical" heading="Dashboard" no-header>
        <div class="flex-2 flex text-centered">
            <DashboardClockComponent />
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import DashboardClockComponent from '@/view/dashboard/components/DashboardClock.component.vue';
</script>

<style lang="scss">
@use '@/styling/variables.scss' as *;

.dashboard-view {

    .dashboard {
        flex: 0;
        opacity: 0;
        transition: all 0.5s;

        &.is-visible {
            flex: 1;
            opacity: 1;
        }
    }
}
</style>@/api/api.use@/models/Dashboard.model
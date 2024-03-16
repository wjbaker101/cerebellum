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
.dashboard-view {
}
</style>
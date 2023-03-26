<template>
    <ViewComponent class="dashboard-view flex flex-vertical" heading="Dashboard" no-header>
        <div class="flex-2 flex text-centered">
            <DashboardClockComponent />
        </div>
        <div class="dashboard" :class="{ 'is-visible': dashboard !== null }">
            <div class="content-width">
                <div class="dashboard-items">
                    <RouterLink class="border text-centered" :key="item.reference" v-for="item in dashboard?.items" :to="mapLink(item)">
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

import GradientBorderComponent from '@/component/GradientBorderComponent.vue';
import DashboardClockComponent from '@/view/dashboard/components/DashboardClock.component.vue';

import { useApi } from '@/use/api/api.use';

import { IDashboard, IDashboardItem } from '@/model/Dashboard.model';

const api = useApi();

const dashboard = ref<IDashboard | null>(null);

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
    dashboard.value = await api.dashboard.get();
});
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
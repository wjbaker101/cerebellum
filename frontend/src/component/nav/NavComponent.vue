<template>
    <nav class="nav-component">
        <h1 class="text-centered">
            Cerebellum
            <NavClockComponent :enabled="!isDashboard" />
        </h1>
        <ul class="tabs">
            <router-link to="/" class="tab">
                <GradientBorderComponent :on-hover="!isDashboard">
                    <IconComponent icon="home" />
                    <div>Dashboard</div>
                </GradientBorderComponent>
            </router-link>
            <router-link to="/calendar" class="tab">
                <GradientBorderComponent :on-hover="!route.path.startsWith('/calendar')">
                    <IconComponent icon="calendar" />
                    <div>Calendar</div>
                </GradientBorderComponent>
            </router-link>
            <router-link to="/notes" class="tab">
                <GradientBorderComponent :on-hover="!route.path.startsWith('/notes')">
                    <IconComponent icon="page-text" />
                    <div>Notes</div>
                </GradientBorderComponent>
            </router-link>
            <router-link to="/listum" class="tab">
                <GradientBorderComponent :on-hover="!route.path.startsWith('/listum')">
                    <IconComponent icon="menu" />
                    <div>Listum</div>
                </GradientBorderComponent>
            </router-link>
        </ul>
    </nav>
    <nav class="nav-placeholder" aria-hidden="true"></nav>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';

import GradientBorderComponent from '@/component/GradientBorderComponent.vue';
import NavClockComponent from '@/component/nav/component/NavClockComponent.vue';

const route = useRoute();

const isDashboard = computed<boolean>(() => route.path === '/');
</script>

<style lang="scss">
@use '~@wjb/styling/variables' as *;
@use '@/styling/variables' as *;

nav.nav-placeholder,
nav.nav-component {
    flex: 0 0 157px;
}

nav.nav-component {
    position: fixed;
    top: 0;
    left: 0;
    bottom: 0;
    padding: 1rem;
    border-top-right-radius: var(--wjb-border-radius);
    border-bottom-right-radius: var(--wjb-border-radius);
    background-color: var(--wjb-background-colour);

    @include shadow-medium();

    .tabs {
        list-style: none;
        padding-left: 0;
        margin: 0%;

        .tab {
            display: block;
            text-transform: uppercase;
            font-size: 0.75rem;
            text-align: center;
            color: inherit;
            text-decoration: none;
            border-radius: var(--wjb-border-radius);
            border: 2px solid transparent;

            & > * {
                padding: 1rem 0;
            }

            & + .tab {
                margin-top: 1rem;
            }
        }
    }
}
</style>
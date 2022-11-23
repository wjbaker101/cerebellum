<template>
    <nav class="nav-component">
        <div class="title-heading flex align-items-center">
            <h1>
                Cerebellum
                <NavClockComponent :enabled="!isDashboard" />
            </h1>
            <div class="open-button-container flex-auto">
                <ButtonComponent class="primary mini" @click="toggleOpen">
                    <IconComponent v-if="isOpen" icon="cross" />
                    <IconComponent v-else icon="menu" />
                </ButtonComponent>
            </div>
        </div>
        <ul class="tabs" :class="{ 'is-open': isOpen }">
            <router-link to="/" class="tab" @click.native="onNavigate">
                <GradientBorderComponent :on-hover="!isDashboard">
                    <IconComponent icon="home" />
                    <div>Dashboard</div>
                </GradientBorderComponent>
            </router-link>
            <router-link to="/calendar" class="tab" @click.native="onNavigate">
                <GradientBorderComponent :on-hover="!route.path.startsWith('/calendar')">
                    <IconComponent icon="calendar" />
                    <div>Calendar</div>
                </GradientBorderComponent>
            </router-link>
            <router-link to="/notes" class="tab" @click.native="onNavigate">
                <GradientBorderComponent :on-hover="!route.path.startsWith('/notes')">
                    <IconComponent icon="page-text" />
                    <div>Notes</div>
                </GradientBorderComponent>
            </router-link>
            <router-link to="/listum" class="tab" @click.native="onNavigate">
                <GradientBorderComponent :on-hover="!route.path.startsWith('/listum')">
                    <IconComponent icon="menu" />
                    <div>Listum</div>
                </GradientBorderComponent>
            </router-link>
            <router-link to="/workout-diary" class="tab" @click.native="onNavigate">
                <GradientBorderComponent :on-hover="!route.path.startsWith('/workout-diary')">
                    <IconComponent icon="flex" />
                    <div>Workout Diary</div>
                </GradientBorderComponent>
            </router-link>
        </ul>
    </nav>
    <nav class="nav-placeholder" aria-hidden="true"></nav>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { useRoute } from 'vue-router';

import GradientBorderComponent from '@/component/GradientBorderComponent.vue';
import NavClockComponent from '@/component/nav/component/NavClockComponent.vue';

const route = useRoute();

const isDashboard = computed<boolean>(() => route.path === '/');

const isOpen = ref<boolean>();

const toggleOpen = function (): void {
    isOpen.value = !isOpen.value;
};

const onNavigate = function (): void {
    isOpen.value = false;
};
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

    .title-heading {
        text-align: center;
    }

    .open-button-container {
        display: none;
    }

    .tabs {
        list-style: none;
        padding-left: 0;
        margin: 0%;
        opacity: 1;
        pointer-events: all;

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

@media screen and (max-width: $breakpoint) {
    nav.nav-component {
        position: relative;
        flex: 0 0 auto;

        .title-heading {
            text-align: left;

            h1 {
                margin-bottom: 0;
            }
        }

        .tabs {
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            padding: 0 1rem 1rem 1rem;
            background-color: var(--wjb-background-colour);
            opacity: 0;
            pointer-events: none;
            z-index: 1;

            @include shadow-medium();

            &.is-open {
                opacity: 1;
                pointer-events: all;
            }
        }

        .open-button-container {
            display: unset;
        }
    }

    nav.nav-placeholder {
        display: none;
    }
}
</style>
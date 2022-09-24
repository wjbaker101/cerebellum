<template>
    <GradientBorderComponent class="page-heading-component flex flex-auto align-items-center gap" v-if="!noHeader">
        <div>
            <slot name="title">
                <h1>{{ heading }}</h1>
            </slot>
        </div>
        <div class="flex-auto">
            <slot></slot>
        </div>
    </GradientBorderComponent>
</template>

<script setup lang="ts">
import GradientBorderComponent from '@/component/GradientBorderComponent.vue';

defineProps<{
    heading: string;
    noHeader?: boolean;
}>();
</script>

<style lang="scss">
.page-heading-component {
    padding: 1rem;

    h1 {
        margin: 0;
    }
}
</style>
<template>
    <div class="app-view">
        <PageHeadingComponent :heading="heading" :no-header="noHeader">
            <slot name="header"></slot>
        </PageHeadingComponent>
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
import PageHeadingComponent from '@/component/PageHeadingComponent.vue';

defineProps<{
    heading: string;
    noHeader: boolean;
}>();
</script>

<style lang="scss">
.app-view {
    padding: 1rem;
}
</style>
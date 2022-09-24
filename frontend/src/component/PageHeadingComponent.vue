<template>
    <div class="page-heading-component flex flex-auto align-items-center" v-if="!noHeader">
        <div>
            <slot name="title">
                <h1>{{ heading }}</h1>
            </slot>
        </div>
        <div class="flex-auto">
            <slot></slot>
        </div>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    heading: string;
    noHeader?: boolean;
}>();
</script>

<style lang="scss">
.page-heading-component {
    padding: 1rem;
    border: 2px solid var(--wjb-primary);
    border-radius: var(--wjb-border-radius);

    h1 {
        margin: 0;
    }
}
</style>
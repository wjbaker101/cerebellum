<template>
    <div class="gradient-border-component" :class="{ 'is-disabled': !!disabled, 'on-hover': !!onHover }">
        <div class="border"></div>
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    disabled?: boolean;
    onHover?: boolean;
}>();
</script>

<style lang="scss">
.gradient-border-component {
    $border: 2px;
    $border-100: calc(100% - $border);

    position: relative;
    border-radius: var(--wjb-border-radius);

    & > .border {
        position: absolute;
        inset: 0;
        padding: $border;
        border-radius: var(--wjb-border-radius);
        background: linear-gradient(135deg, var(--wjb-primary), var(--wjb-secondary));
        mask: linear-gradient(#fff 0 0) content-box, linear-gradient(#fff 0 0);
        mask-composite: exclude;
    }

    &.is-disabled,
    &.on-hover {
        & > .border {
            background: var(--wjb-background-colour);
            opacity: 0;
        }
    }

    &.on-hover:hover {
        & > .border {
            background: linear-gradient(135deg, var(--wjb-primary), var(--wjb-secondary));
            opacity: 1;
        }
    }
}
</style>
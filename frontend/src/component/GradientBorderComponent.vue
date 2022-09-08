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
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        border-radius: var(--wjb-border-radius);
        background: linear-gradient(135deg, var(--wjb-primary), var(--wjb-secondary));
        clip-path: polygon(0% 0%, 0% 100%, $border 100%, $border $border, $border-100 $border, $border-100 $border-100, 0 $border-100, 0 100%, 100% 100%, 100% 0%);
        opacity: 1;
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
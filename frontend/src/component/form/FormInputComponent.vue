<template>
    <label class="@wjb-form-input-component" :class="{ 'is-required': required }">
        <strong class="label">{{ label }}</strong>
        <slot></slot>
    </label>
</template>

<script setup lang="ts">
defineProps<{
    label: string;
    required?: boolean;
}>();
</script>

<style lang="scss" scoped>
.\@wjb-form-input-component {
    display: block;

    &.is-required {
        .label {
            &::after {
                content: '*';
            }
        }
    }

    .label {
        display: block;
        line-height: 1em;
        margin-bottom: 0.25rem;
    }

    & + .\@wjb-form-input-component {
        margin-top: 1rem;
    }
}
</style>
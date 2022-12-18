<template>
    <input
        ref="element"
        class="hidden-textbox-component"
        type="text"
        :value="modelValue"
        @input="onInput"
        @blur="onFinish"
        @keypress.enter="element?.blur()"
    >
</template>

<script setup lang="ts">
import { ref } from 'vue';

defineProps<{
    modelValue: string;
}>();

const emit = defineEmits(['update:modelValue', 'finish']);

const element = ref<HTMLInputElement | null>(null);

const onInput = function (event: Event): void {
    emit('update:modelValue', (event.target as HTMLInputElement).value);
};

const onFinish = function (): void {
    emit('finish', element.value?.value);
};
</script>

<style lang="scss">
.hidden-textbox-component {
    padding: 0.25rem;

    &:not(:focus) {
        box-shadow: none;
    }
}
</style>
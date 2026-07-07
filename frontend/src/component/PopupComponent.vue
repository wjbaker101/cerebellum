<template>
    <transition name="@wjb-popup-transition">
        <div v-if="isVisible" class="@wjb-popup-component" :class="{ [`style-${details.style}`]: true, }" @click="onClick">
            <IconComponent v-if="details.style === 'error'" icon="cross" gap="right" />
            <IconComponent v-if="details.style === 'success'" icon="tick" gap="right" />
            <span>{{ details.message }}</span>
        </div>
    </transition>
</template>

<script setup lang="ts">
import IconComponent from './IconComponent.vue';

import { usePopup } from '@/composables/popup.use';

const popup = usePopup();

const details = popup.details;
const isVisible = popup.isVisible;

const onClick = function (): void {
    popup.hide();
};
</script>

<style lang="scss" scoped>
.\@wjb-popup-component {
    position: fixed;
    top: 1rem;
    left: 50%;
    padding: 1rem;
    border-radius: var(--wjb-border-radius);
    transform: translateX(-50%);
    z-index: 15;
    cursor: pointer;
    transition: transform 0.3s, opacity 0.3s;

    &.style-error {
        background-color: var(--wjb-danger);
    }

    &.style-success {
        background-color: var(--wjb-success);
    }

    &.style-warning {
        background-color: var(--wjb-warning);
    }
}

.popup-transition-enter-from,
.popup-transition-leave-to {
    transform: translateX(-50%) translateY(calc(-100% - 1rem)) scale(0.2);
    opacity: 0;
}
</style>
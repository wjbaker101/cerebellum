<template>
    <div
        class="@wjb-modal-component"
        :class="classes"
        @click.self="onClose"
    >
        <transition name="modal-transition">
            <div v-if="isVisible" class="content-container">
                <div class="close" @click="onClose">
                    <IconComponent icon="cross" />
                </div>
                <component :is="component" v-bind="componentProps" />
            </div>
        </transition>
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, shallowRef } from 'vue';

import IconComponent from './IconComponent.vue';

import { type ModalStyle, useModal } from '@/composables/modal.use';

const props = defineProps<{
    style?: ModalStyle,
}>();

const modal = useModal();

const style = ref<ModalStyle>(props.style ?? 'centered');
const component = shallowRef<any>();
const componentProps = ref<object>({});
const isVisible = ref<boolean>(false);
const onCloseCallback = ref<(() => void) | undefined>(undefined);

const classes = computed<Record<string, boolean>>(() => ({
    'is-visible': isVisible.value,
    [`style-${style.value}`]: true,
}));

onMounted(() => {
    modal.setOnShow(details => {
        style.value = details.style ?? 'centered';
        component.value = details.component;
        componentProps.value = details.componentProps;
        isVisible.value = true;
        onCloseCallback.value = details.onClose;
    });

    modal.setOnHide(() => {
        isVisible.value = false;

        if (onCloseCallback.value)
            onCloseCallback.value();
    });
});

const onClose = function () {
    modal.hide();
};
</script>

<style lang="scss">
body.wjb-modal-visible {
    overflow-y: hidden;
}
</style>

<style lang="scss" scoped>
.\@wjb-modal-component {
    display: grid;
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    padding: 0.5rem;
    background-color: var(--wjb_modal-backdrop-colour, rgba(0, 0, 0, 0.5));
    z-index: 10;
    opacity: 0;
    pointer-events: none;
    overflow-y: auto;
    overflow-x: hidden;

    &.style-filled {
        & > .content-container {
            top: 1rem;
            left: 1rem;
            bottom: 1rem;
            right: 1rem;
        }
    }

    &.style-centered {
        & > .content-container {
            position: relative;
            margin: auto;
            max-width: 100%;
        }

        .modal-transition-enter-from {
            transform: translateY(-20rem) scale(0.2);
        }

        .modal-transition-leave-to {
            transform: translateY(20rem) scale(0.2);
        }
    }

    &.style-only-background {
        & > .content-container {
            height: 100%;
            border-radius: 0;
            background-color: transparent;
        }
    }

    &.style-fullscreen {
        & > .content-container {
            width: 100%;
            height: 100%;
            padding: 0;
            border-radius: 0;
            background-color: transparent;
        }
    }

    &.style-side-right {
        grid-template-columns: 1fr minmax(30%, 450px);

        & > .content-container {
            margin-right: -0.5rem;
            grid-column-start: 2;
            border-radius: 0;
            border-top-left-radius: var(--wjb-border-radius);
            border-bottom-left-radius: var(--wjb-border-radius);
        }

        .modal-transition-enter-from {
            transform: translateX(100%);
        }

        .modal-transition-leave-to {
            transform: translateX(100%);
        }
    }

    &.is-visible {
        opacity: 1;
        pointer-events: all;
    }

    & > .content-container {
        padding: 1rem;
        background-color: var(--wjb_modal-content-background-colour, #efefef);
        border-radius: 0.3rem;
        overflow-y: auto;
        overflow-x: hidden;
    }

    & > .content-container > .close {
        padding: 0.5rem;
        position: absolute;
        top: 0.5rem;
        right: 0.5rem;
        z-index: 10;
        line-height: 1em;
        background-color: inherit;
        border-radius: 0.3rem;
        cursor: pointer;

        &:hover {
            box-shadow: 1px 2px 3px rgba(0, 0, 0, 0.4);
        }
    }
}
</style>
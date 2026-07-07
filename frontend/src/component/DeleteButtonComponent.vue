<template>
    <ButtonComponent class="@wjb-delete-button-component danger" :class="classes" @click="onClick" @blur="onBlur">
        <IconComponent v-if="!onlyIcon || onlyIcon && !isConfirming" icon="bin" :gap="onlyIcon ? 'none' : 'right'" />
        <IconComponent v-if="onlyIcon && isConfirming" icon="tick" />
        <span v-if="!onlyIcon">{{ isConfirming ? 'Confirm' : 'Delete' }}</span>
    </ButtonComponent>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';

import ButtonComponent from './ButtonComponent.vue';
import IconComponent from './IconComponent.vue';

export default defineComponent({
    name: 'DeleteButtonComponent',

    components: {
        ButtonComponent,
        IconComponent,
    },

    props: {
        onlyIcon: {
            type: Boolean,
            default: false,
            required: false,
        },
    },

    emits: [ 'delete' ],

    setup(props, { emit }) {
        const isConfirming = ref<boolean>(false);

        const classes: Record<string, boolean> = {
            'mini': props.onlyIcon
        };

        return {
            classes,
            isConfirming,

            onClick() {
                if (isConfirming.value) {
                    isConfirming.value = false;
                    emit('delete');
                    return;
                }

                isConfirming.value = true;
            },

            onBlur() {
                isConfirming.value = false;
            },
        };
    },
});
</script>

<style lang="scss" scoped>
</style>
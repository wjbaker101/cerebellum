<template>
    <ViewComponent class="note-view flex flex-vertical gap" heading="Note">
        <div class="text-container flex">
            <div class="line-numbers flex-auto text-right">
                <div v-for="i in rowCount">{{ i }}</div>
            </div>
            <textarea :rows="rowCount" v-model="noteContent"></textarea>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const noteReference = route.params.noteReference;

const noteContent = ref<string>('');

const rowCount = computed<number>(() => noteContent.value.split('\n').length);
</script>

<style lang="scss">
@use '~@wjb/styling/variables.scss' as *;

.note-view {

    .line-numbers {
        padding: 0.5rem;
        color: #999;
    }

    .text-container {
        background-color: var(--wjb-background-colour-dark);

        @include shadow-small();
    }

    textarea {
        background-color: transparent;
        box-shadow: none;

        &:focus {
            outline: none;
        }
    }
}
</style>
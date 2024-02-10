<template>
    <ViewComponent class="note-view flex flex-vertical gap" :heading="note?.title ?? ''">
        <template v-slot:title v-if="isEditingTitle">
            <h1>
                <input ref="titleInput" type="text" v-model="noteTitle" @keypress.enter="onTitleConfirm">
            </h1>
        </template>
        <template v-slot:header>
            <div class="flex gap-small">
                <div class="flex-auto">
                    <ButtonComponent class="primary" @click="onEditTitle">
                        <template v-if="isEditingTitle">
                            <IconComponent icon="cross" gap="right" />
                            <span>Cancel</span>
                        </template>
                        <template v-else>
                            <IconComponent icon="pencil" gap="right" />
                            <span>Edit Title</span>
                        </template>
                    </ButtonComponent>
                </div>
                <div class="flex-auto">
                    <DeleteButtonComponent @delete="onDelete" />
                </div>
            </div>
        </template>
        <div class="text-container flex">
            <div class="line-numbers flex-auto text-right">
                <div v-for="i in rowCount">{{ i }}</div>
            </div>
            <textarea :rows="rowCount" v-model="noteContent"></textarea>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, nextTick, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { debounce } from 'ts-debounce';

import { useApi } from '@/api/api.use';

import { INote } from '@/model/Note.model';

const api = useApi();
const route = useRoute();
const router = useRouter();

const noteReference = route.params.noteReference as string;

const titleInput = ref<HTMLInputElement | null>(null);

const note = ref<INote | null>(null);

const onNoteUpdate = debounce(async () => {
    if (note.value === null)
        return;

    await api.notes.updateNote(note.value.reference, {
        title: note.value.title,
        content: note.value.content,
    });
}, 200);

const noteTitle = ref<string>('');

const noteContent = computed<string>({
    get() {
        return note.value?.content ?? '';
    },
    set(value: string) {
        if (note.value === null)
            return;

        note.value.content = value;

        onNoteUpdate();
    },
});
const rowCount = computed<number>(() => noteContent.value.split('\n').length);

const isEditingTitle = ref<boolean>(false);

const onEditTitle = function () {
    if (isEditingTitle.value) {
        isEditingTitle.value = false;
        return;
    }

    isEditingTitle.value = true;
    noteTitle.value = note.value?.title ?? '';

    nextTick(() => {
        titleInput.value?.focus();
    });
};

const onTitleConfirm = function () {
    if (note.value === null)
        return;

    isEditingTitle.value = false;
    note.value.title = noteTitle.value;

    onNoteUpdate();
};

const onDelete = function () {
    if (note.value === null)
        return;

    api.notes.deleteNote(note.value.reference);

    router.push({ path: '/notes' });
};

onMounted(async () => {
    const result = await api.notes.getNote(noteReference);

    note.value = result;
});
</script>

<style lang="scss">
@use '@/styling/variables' as *;

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
</style>@/api/api.use
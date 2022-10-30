<template>
    <ViewComponent class="list-view flex flex-vertical gap" :heading="listum?.title ?? ''">
        <template v-slot:title v-if="isEditingTitle">
            <h1>
                <input ref="titleInput" type="text" v-model="listTitle" @keypress.enter="onTitleConfirm">
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
    </ViewComponent>
</template>

<script setup lang="ts">
import { nextTick, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { debounce } from 'ts-debounce';

import { useApi } from '@/use/api/api.use';

import { IListum } from '@/model/Listum.model';

const api = useApi();
const route = useRoute();
const router = useRouter();

const listumReference = route.params.listumReference as string;

const titleInput = ref<HTMLInputElement | null>(null);

const listum = ref<IListum | null>(null);

const listTitle = ref<string>('');

const isEditingTitle = ref<boolean>(false);

const onListUpdate = debounce(async () => {
    if (listum.value === null)
        return;

    await api.listum.updateList(listum.value.reference, {
        title: listum.value.title,
    });
}, 200);

const onEditTitle = function () {
    if (isEditingTitle.value) {
        isEditingTitle.value = false;
        return;
    }

    isEditingTitle.value = true;
    listTitle.value = listum.value?.title ?? '';

    nextTick(() => {
        titleInput.value?.focus();
    });
};

const onTitleConfirm = function () {
    if (listum.value === null)
        return;

    isEditingTitle.value = false;
    listum.value.title = listTitle.value;

    onListUpdate();
};

const onDelete = function () {
    if (listum.value === null)
        return;

    router.push({ path: '/listum' });
};

onMounted(async () => {
    const result = await api.listum.getListByReference(listumReference);

    listum.value = result;
});
</script>

<style lang="scss">
@use '~@wjb/styling/variables.scss' as *;

.list-view {
}
</style>
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
        <div class="list" v-if="listum">
            <div class="add-item-container flex gap-small align-items-center">
                <div>
                    <input type="text" v-model="newItemContent" placeholder="Item contents...">
                </div>
                <div class="flex-auto">
                    <ButtonComponent class="primary" title="New List Item" @click="onNewItem">
                        <IconComponent icon="plus" />
                    </ButtonComponent>
                </div>
            </div>
            <ol>
                <li v-for="item in listum.items" class="flex align-items-center gap">
                    <div>
                        {{ item.content }}
                    </div>
                    <div class="flex-auto">
                        <ButtonComponent class="mini" @click="onItemClick(item)">
                            <IconComponent icon="menu" />
                        </ButtonComponent>
                    </div>
                </li>
            </ol>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { nextTick, onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { debounce } from 'ts-debounce';

import ListItemModalComponent from '@/view/listum/modal/ListItemModal.component.vue';

import { useModal } from '@wjb/vue/use/modal.use';
import { useApi } from '@/use/api/api.use';

import { IListum, IListumItem } from '@/model/Listum.model';

const api = useApi();
const route = useRoute();
const router = useRouter();
const modal = useModal();

const listumReference = route.params.listumReference as string;

const titleInput = ref<HTMLInputElement | null>(null);
const listum = ref<IListum | null>(null);
const listTitle = ref<string>('');
const isEditingTitle = ref<boolean>(false);
const newItemContent = ref<string>('');

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

const onNewItem = async function (): Promise<void> {
    if (listum.value === null)
        return;

    const listItem = await api.listum.addListItem(listum.value.reference, {
        content: newItemContent.value,
    });

    listum.value.items.push(listItem);
};

const onItemClick = function(listItem: IListumItem): void {
    if (listum.value === null)
        return;

    modal.show({
        component: ListItemModalComponent,
        componentProps: {
            listReference: listum.value.reference,
            listItem,
        },
    });
};

onMounted(async () => {
    const result = await api.listum.getListByReference(listumReference);

    listum.value = result;
});
</script>

<style lang="scss">
@use '~@wjb/styling/variables.scss' as *;

.list-view {
    .list {
        max-width: 720px;
        margin: auto;
    }

    .add-item-container {
        width: 720px;
        max-width: 720px;
        padding: 0.5rem;
        border-radius: var(--wjb-border-radius);
        border: 1px solid var(--wjb-tertiary);
    }

    ol {
        counter-reset: count;
        list-style: none;
        padding-left: 0;

        li {
            padding: 0.25rem;
            counter-increment: count;

            @include shadow-small();

            &::before {
                content: counter(count);
                width: 2rem;
                display: inline-block;
                text-align: right;
            }

            & + li {
                margin-top: 0.25rem;
            }
        }
    }
}
</style>
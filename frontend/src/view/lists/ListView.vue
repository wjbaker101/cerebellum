<template>
    <ViewComponent v-if="list" class="list-view flex flex-vertical gap" :heading="list.title">
        <template v-slot:title>
            <h1>
                <HiddenTextboxComponent v-model="listTitle" @finish="onTitleConfirm" />
            </h1>
        </template>
        <template v-slot:header>
            <div class="flex gap-small">
                <div class="flex-auto">
                    <DeleteButtonComponent @delete="onDelete" />
                </div>
            </div>
        </template>
        <div class="list" v-if="list">
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
                <li v-for="item in list.items" class="flex align-items-center gap">
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
import { onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { debounce } from 'ts-debounce';

import HiddenTextboxComponent from '@/component/HiddenTextboxComponent.vue';
import ListItemModalComponent, { IListItemModalProps } from '@/view/lists/modal/ListItemModal.component.vue';

import { useModal } from '@wjb/vue/use/modal.use';
import { useApi } from '@/api/api.use';

import { IList, IListItem } from '@/view/lists/model/List.model';

const api = useApi();
const route = useRoute();
const router = useRouter();
const modal = useModal();

const listReference = route.params.listReference as string;

const list = ref<IList | null>(null);
const listTitle = ref<string>('');
const isEditingTitle = ref<boolean>(false);
const newItemContent = ref<string>('');

const onListUpdate = debounce(async () => {
    if (list.value === null)
        return;

    await api.lists.updateList(list.value.reference, {
        title: list.value.title,
    });
}, 200);

const onTitleConfirm = function () {
    if (list.value === null)
        return;

    isEditingTitle.value = false;
    list.value.title = listTitle.value;

    onListUpdate();
};

const onDelete = function () {
    if (list.value === null)
        return;

    router.push({ path: '/lists' });
};

const onNewItem = async function (): Promise<void> {
    if (list.value === null)
        return;

    const listItem = await api.lists.addListItem(list.value.reference, {
        content: newItemContent.value,
    });

    list.value.items.push(listItem);
};

const onItemClick = function(listItem: IListItem): void {
    if (list.value === null)
        return;

    modal.show<IListItemModalProps>({
        component: ListItemModalComponent,
        componentProps: {
            listReference: list.value.reference,
            listItem,
        },
    });
};

onMounted(async () => {
    const result = await api.lists.getListByReference(listReference);

    list.value = result;
    listTitle.value = result.title;
});
</script>

<style lang="scss">
@use '@/styling/variables' as *;

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
                content: counter(count) ".";
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
</style>@/api/api.use
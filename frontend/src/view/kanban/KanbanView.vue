<template>
    <ViewComponent class="kanban-view" heading="Kanban Boards">
        <template #header>
            <ButtonComponent class="primary" @click="onCreate">
                <IconComponent icon="plus" gap="right" />
                <span>Create</span>
            </ButtonComponent>
        </template>
        <ListComponent :items="kanbanBoards" itemName="boards" :urlMapper="(x: IKanbanBoard) => `/kanban/${x.reference}`">
            <template #item="{ item }">
                {{ item.title }}
            </template>
        </ListComponent>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

import ListComponent from '@/component/ListComponent.vue';

import { useApi } from '@/use/api/api.use';

import { IKanbanBoard } from '@/view/kanban/model/KanbanBoard.model';

const api = useApi();
const router = useRouter();

const kanbanBoards = ref<Array<IKanbanBoard> | null>(null);

const onCreate = async function () {
    // const list = await api.kanban.cr({
    //     title: 'New List',
    // });

    // kanbanBoards.value?.push(list);
    // router.push({ path: `/listum/${list.reference}`, });
};

onMounted(async () => {
    const response = await api.kanban.search();

    kanbanBoards.value = response;
});
</script>

<style lang="scss">
</style>
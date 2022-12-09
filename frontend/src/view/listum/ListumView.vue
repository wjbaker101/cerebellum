<template>
    <ViewComponent class="listum-view" heading="Listum" :no-header="false">
        <template #header>
            <ButtonComponent class="primary" @click="onCreate">
                <IconComponent icon="plus" gap="right" />
                <span>Create</span>
            </ButtonComponent>
        </template>
        <ListComponent :items="lists" itemName="lists" :urlMapper="(x: IListum) => `/listum/${x.reference}`">
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

import { IListum } from '@/model/Listum.model';

const api = useApi();
const router = useRouter();

const lists = ref<Array<IListum> | null>(null);

const onCreate = async function () {
    const list = await api.listum.createList({
        title: 'New List',
    });

    lists.value?.push(list);
    router.push({ path: `/listum/${list.reference}`, });
};

onMounted(async () => {
    const response = await api.listum.search();

    lists.value = response;
});
</script>

<style lang="scss">
</style>
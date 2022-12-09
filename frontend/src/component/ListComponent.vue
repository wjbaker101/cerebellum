<template>
    <LoadingComponent :itemName="itemName" v-if="items === null" />
    <NoItemsComponent :itemName="itemName" v-else-if="items.length === 0" />
    <ul v-else class="list-component" :class="{ 'remove-styling': removeStyling }">
        <template v-for="item in items">
            <router-link v-if="urlMapper" :to="urlMapper(item)">
                <slot name="item" :item="item"></slot>
            </router-link>
            <li v-else>
                <slot name="item" :item="item"></slot>
            </li>
        </template>
    </ul>
</template>

<script setup lang="ts">
import LoadingComponent from '@/component/LoadingComponent.vue';
import NoItemsComponent from '@/component/NoItemsComponent.vue';

type ItemType = any;

defineProps<{
    items: Array<ItemType> | null;
    itemName?: string;
    urlMapper?: (x: ItemType) => string;
    removeStyling?: boolean;
}>();
</script>

<style lang="scss">
.list-component {
    width: 720px;
    max-width: 100%;
    margin: 1rem auto 1rem auto;
    padding-left: 0;
    list-style: none;

    & > a,
    & > li {
        display: block;
        padding: 0.5rem;
        border: 2px solid var(--wjb-background-colour-dark);
        border-radius: var(--wjb-border-radius);
        color: inherit;
        text-decoration: none;

        & + a,
        & + li {
            margin-top: 0.5rem;
        }
    }

    &.remove-styling {
        & > a,
        & > li {
            padding: 0;
            border: none;
        }
    }
}
</style>
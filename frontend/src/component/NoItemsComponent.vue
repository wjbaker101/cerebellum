<template>
    <div class="no-items-component">
        <IconComponent icon="bin" size="large" />
        <p>No {{ displayItemName }} yet!</p>
    </div>
</template>

<script setup lang="ts">
const props = defineProps<{
    itemName?: string;
}>();

const displayItemName = props.itemName ?? 'items';
</script>

<style lang="scss">
</style>
<template>
    <div class="no-items-component text-centered">
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
.no-items-component {
    margin: 2rem;
}
</style>
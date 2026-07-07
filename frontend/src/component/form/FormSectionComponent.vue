<template>
    <div class="@wjb-form-section-component">
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
</script>

<style lang="scss" scoped>
.\@wjb-form-section-component {
    padding: 1rem;
    background-color: var(--wjb-background-colour-dark);
    border-radius: var(--wjb-border-radius);
}
</style>
<template>
    <div class="page flex breakpoint">
        <NavComponent></NavComponent>
        <router-view></router-view>
        <ModalComponent />
        <PopupComponent />
    </div>
</template>

<script setup lang="ts">
import ModalComponent from '@wjb/vue/component/ModalComponent.vue';
import PopupComponent from '@wjb/vue/component/PopupComponent.vue';
import NavComponent from '@/component/nav/NavComponent.vue';
</script>

<style lang="scss">
@use '@/styling/main' as *;
@use '@/styling/variables' as *;

*,
*::before,
*::after {
    box-sizing: border-box;
    vertical-align: middle;
    transition: all 0.2s, font 0s;
}

html,
body,
#app,
#app > .page {
    height: 100%;
}

body {
    margin: 0;
    font-family: 'Nunito', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    font-size: 15px;
    line-height: 1.6em;
    letter-spacing: 0.5px;
    background-color: var(--wjb-background-colour);
    color: var(--wjb-text-colour);
    transition: none;
}

textarea {
    width: 100%;
    max-width: 100%;
    padding: 0.5rem;
    transition: none;
    background-color: var(--wjb-background-colour-dark);
    color: inherit;
    border: none;
    font: inherit;
    letter-spacing: inherit;
    border-radius: var(--wjb-border-radius);

    @include shadow-small();
}

.content-width {
    max-width: 67rem;
    margin-left: auto;
    margin-right: auto;
}
</style>
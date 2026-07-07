<template>
    <button class="@wjb-button-component" :disabled="loading">
        <slot v-if="!loading"></slot>
        <template v-else>
            <svg
                width="15"
                height="15"
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                viewBox="0 0 100 100"
                preserveAspectRatio="xMidYMid"
                class="loading-icon"
            >
                <circle cx="50" cy="50" fill="none" stroke="currentColor" stroke-width="10" r="47" stroke-dasharray="115 80" stroke-linecap="round">
                    <animateTransform attributeName="transform" type="rotate" repeatCount="indefinite" dur="1s" values="0 50 50;360 50 50" keyTimes="0;1" />
                </circle>
            </svg>
            <span class="loading-text">Loading</span>
        </template>
    </button>
</template>

<script setup lang="ts">
defineProps<{
    loading?: boolean;
}>();
</script>

<style lang="scss" scoped>
@mixin style($colour, $textColour: "var(--wjb-text-colour)") {
    background-color: var(--wjb-#{$colour});
    color: #{$textColour};

    &:focus,
    &:active {
        outline: 2px dashed var(--wjb-#{$colour});
    }

    &:hover {
        background-color: var(--wjb-#{$colour}-dark);
    }
}

.\@wjb-button-component {
    padding: 0.5rem 1rem;
    border-radius: 0.3rem;
    border: 0;
    cursor: pointer;
    vertical-align: middle;
    font: inherit;
    letter-spacing: inherit;
    box-shadow: 1px 2px 4px rgba(0, 0, 0, 0.4);
    outline-offset: 1px;
    outline: 2px dashed transparent;

    @include style("primary");

    &.mini {
        padding: 0.25rem 0.5rem;
    }

    &.secondary { @include style("secondary", "var(--wjb-text-colour-opposite)"); }
    &.tertiary { @include style("tertiary"); }
    &.danger { @include style("danger"); }
    &.plain { @include style("grey", "var(--wjb-text-colour-opposite)"); }

    :deep(span) {
        vertical-align: middle;
    }

    .loading-icon {
        margin-right: 0.5rem;
        filter: drop-shadow(1px 1px 6px rgba(0, 0, 0, 0.7));
        vertical-align: middle;
        animation: button-loading-component-animation 5s alternate infinite;

        @keyframes button-loading-component-animation {
            0% {
                color: var(--wjb-primary);
            }

            50% {
                color: var(--wjb-secondary);
            }

            100% {
                color: var(--wjb-tertiary);
            }
        }
    }

    .loading-text {
        vertical-align: unset;
    }
}
</style>
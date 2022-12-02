<template>
    <div class="loading-component text-centered">
        <svg
            xmlns="http://www.w3.org/2000/svg"
            xmlns:xlink="http://www.w3.org/1999/xlink"
            viewBox="0 0 100 100"
            preserveAspectRatio="xMidYMid"
        >
            <circle cx="50" cy="50" fill="none" stroke="currentColor" stroke-width="5" r="47" stroke-dasharray="115 80" stroke-linecap="round">
                <animateTransform attributeName="transform" type="rotate" repeatCount="indefinite" dur="1s" values="0 50 50;360 50 50" keyTimes="0;1" />
            </circle>
        </svg>
        <p>Loading {{ itemName }}</p>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    itemName?: string;
}>();
</script>

<style lang="scss">
.loading-component {
    margin: 2rem;

    svg {
        $size: 70px;

        width: $size;
        height: $size;
        color: var(--wjb-primary);
        animation: loading-component-animation 5s alternate infinite;

        @keyframes loading-component-animation {
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
}
</style>
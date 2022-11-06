<template>
    <ViewComponent class="workout-diary-view flex flex-vertical gap" heading="Workout Diary">
        <template v-slot:header>
            <ButtonComponent class="primary">
                <IconComponent icon="plus" gap="right" />
                <span>New Entry</span>
            </ButtonComponent>
        </template>
        <div class="entry-list-container">
            <div class="entry-item">
                <div class="flex gap align-items-center">
                    <h3>Thurs 12th Feb</h3>
                    <div class="flex-auto">
                        <ButtonComponent class="mini">
                            <IconComponent icon="menu" />
                        </ButtonComponent>
                    </div>
                </div>
                <div class="flex gap align-items-center">
                    <p>
                        <strong>Time: </strong>
                        <span>3pm</span>
                    </p>
                    <p>
                        <strong>Duration: </strong>
                        <span>50m</span>
                    </p>
                    <p>
                        <strong>Weight: </strong>
                        <span>77.2kg</span>
                    </p>
                </div>
                <div>
                    <p>
                        <strong>Decline DB: </strong>
                        <span>2x20kg 10x17.5kg 8x22.5kg</span>
                    </p>
                    <p>
                        <strong>Cable Lateral Raise: </strong>
                        <span>10x6.8kg 10x6.8kg 10x6.8kg</span>
                    </p>
                </div>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
@use '~@wjb/styling/variables' as *;

.workout-diary-view {
    .entry-list-container {
        width: 720px;
        max-width: auto;
        margin: auto;
    }

    .entry-item {
        padding: 0.5rem;

        @include shadow-small();

        & > div + div {
            border-top: 1px solid var(--wjb-secondary)
        }
    }
}
</style>
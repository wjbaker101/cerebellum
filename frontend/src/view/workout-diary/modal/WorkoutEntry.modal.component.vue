<template>
    <div class="workout-entry-modal-component">
        <h2>Edit List Items</h2>
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Date">
                    <input type="text">
                </FormInputComponent>
            </FormSectionComponent>
            <FormSectionComponent class="flex gap">
                <div>
                    <FormInputComponent label="Start Time">
                        <input type="text" placeholder="Start Time">
                    </FormInputComponent>
                </div>
                <div>
                    <FormInputComponent label="End Time">
                        <input type="text" placeholder="End Time">
                    </FormInputComponent>
                </div>
                <div>
                    <FormInputComponent label="Weight (kg)">
                        <input type="text" placeholder="Weight (kg)">
                    </FormInputComponent>
                </div>
            </FormSectionComponent>
            <div class="flex gap align-items-center">
                <h3>Sets:</h3>
                <div class="flex-auto">
                    <ButtonComponent class="primary mini">
                        <IconComponent icon="plus" />
                    </ButtonComponent>
                </div>
            </div>
            <FormSectionComponent class="flex gap">
                <div>
                    <FormInputComponent label="Name">
                        <input type="text" placeholder="Start Time">
                    </FormInputComponent>
                </div>
                <div class="flex-auto">
                    <FormInputComponent label="Reps">
                        <input type="text" placeholder="End Time">
                    </FormInputComponent>
                </div>
                <div class="flex-auto">
                    <FormInputComponent>
                        &times;
                    </FormInputComponent>
                </div>
                <div class="flex-auto">
                    <FormInputComponent label="Weight (kg)">
                        <input type="text" placeholder="Weight (kg)">
                    </FormInputComponent>
                </div>
                <div class="flex-auto">
                    <FormInputComponent>
                        <ButtonComponent class="primary mini">
                            <IconComponent icon="plus" />
                        </ButtonComponent>
                    </FormInputComponent>
                </div>
            </FormSectionComponent>
            <FormSectionComponent>
                <FormInputComponent>
                    <ButtonComponent class="primary">
                        <IconComponent icon="tick" gap="right" />
                        <span>Confirm</span>
                    </ButtonComponent>
                </FormInputComponent>
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.workout-entry-modal-component {
    width: 650px;
    max-width: 100%;
}
</style>
<template>
    <div class="log-in-view">
        <input type="text" v-model="username">
        <input type="password" v-model="password">
        <ButtonComponent @click="onLogIn">Log In</ButtonComponent>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import dayjs from 'dayjs';

import { useApi } from '@/api/api.use';
import { useAuth } from '@/use/auth/Auth.use';

const api = useApi();
const auth = useAuth();

const username = ref<string>('');
const password = ref<string>('');

const onLogIn = async () => {
    const result = await api.auth.logIn({
        username: username.value,
        password: password.value,
    });

    if (result instanceof Error)
        return;

    auth.set({
        loginToken: result.loginToken,
        user: {
            reference: result.user.reference,
            createdAt: dayjs(result.user.createdAt),
            username: result.user.username,
        },
    });
};
</script>

<style lang="scss">
.log-in-view {
}
</style>
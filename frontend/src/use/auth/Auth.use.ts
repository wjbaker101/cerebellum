import { readonly, ref } from 'vue';
import { Dayjs } from 'dayjs';

interface IAuth {
    readonly loginToken: string;
    readonly user: {
        readonly reference: string;
        readonly createdAt: Dayjs;
        readonly username: string;
    };
}

const auth = ref<IAuth | null>(null);

export const useAuth = function () {
    return {

        details: readonly(auth),

        set(newAuth: IAuth) {
            auth.value = newAuth;
        },

        clear() {
            auth.value = null;
        },

    };
};
import { readonly, ref } from 'vue';
import { Dayjs } from 'dayjs';

import { useCache } from '@/use/cache/Cache.use.ts';

const cache = useCache();

interface IAuth {
    readonly loginToken: string;
    readonly user: {
        readonly reference: string;
        readonly createdAt: Dayjs;
        readonly username: string;
    };
}

const auth = ref<IAuth | null>(cache.get<IAuth>('auth'));

export const useAuth = function () {
    return {

        details: readonly(auth),

        set(newAuth: IAuth) {
            auth.value = newAuth;
            cache.set('auth', auth.value);
        },

        clear() {
            auth.value = null;
        },

    };
};
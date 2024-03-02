interface ICacheItem<TData> {
    readonly data: TData;
    readonly expiresAt?: number;
}

export const useCache = function () {
    return {

        get<TData>(key: string): TData | null {
            const storage = localStorage.getItem(key);
            if (storage === null)
                return null;

            const cacheItem = JSON.parse(storage) as ICacheItem<TData>;
            if (cacheItem.expiresAt && cacheItem.expiresAt < Date.now())
                return null;

            return cacheItem.data;
        },

        set<TData>(key: string, data: TData, expiresAt?: number): void {
            const cacheItem: ICacheItem<TData> = {
                data,
                expiresAt,
            };

            localStorage.setItem(key, JSON.stringify(cacheItem));
        },

        remove(key: string): void {
            localStorage.removeItem(key);
        },

    };
};
export const recordHelper = {

    toRecord<T>(values: Array<T>, keyFunc: (value: T) => string, valueFunc: (value: T, index: number) => any): Record<string, any> {
        const object: Record<string, any> = {};

        for (let index = 0; index < values.length; ++index) {
            object[keyFunc(values[index])] = valueFunc(values[index], index);
        }

        return object;
    },

};
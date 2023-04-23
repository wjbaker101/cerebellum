import { App, Plugin } from 'vue';

import { components as wjbComponents } from '@wjb/vue/setup/components';

import ViewComponent from '@/component/ViewComponent.vue';

export const components: Plugin = {

    install(app: App) {
        app.use(wjbComponents);

        app.component('ViewComponent', ViewComponent);
    },

};
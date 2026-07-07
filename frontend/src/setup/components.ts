import { App, Plugin } from 'vue';

import ViewComponent from '@/component/ViewComponent.vue';

export const components: Plugin = {

    install(app: App) {
        app.component('ViewComponent', ViewComponent);
    },

};
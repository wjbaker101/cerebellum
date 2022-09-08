import { App, Plugin } from 'vue';

import ButtonComponent from '@wjb/vue/component/ButtonComponent.vue';
import FormComponent from '@wjb/vue/component/form/FormComponent.vue';
import FormInputComponent from '@wjb/vue/component/form/FormInputComponent.vue';
import FormSectionComponent from '@wjb/vue/component/form/FormSectionComponent.vue';
import IconComponent from '@wjb/vue/component/IconComponent.vue';
import ViewComponent from '@/component/ViewComponent.vue';

export const components: Plugin = {
    install(app: App) {
        app.component('ButtonComponent', ButtonComponent);
        app.component('FormComponent', FormComponent);
        app.component('FormInputComponent', FormInputComponent);
        app.component('FormSectionComponent', FormSectionComponent);
        app.component('IconComponent', IconComponent);
        app.component('ViewComponent', ViewComponent);
    },
};
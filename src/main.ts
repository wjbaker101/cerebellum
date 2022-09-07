import { createApp } from 'vue';
import dayjs from 'dayjs';

import advancedFormat from 'dayjs/plugin/advancedFormat';
import isToday from 'dayjs/plugin/isToday';
import isSameOrAfter from 'dayjs/plugin/isSameOrAfter';
import isSameOrBefore from 'dayjs/plugin/isSameOrBefore';

dayjs.extend(advancedFormat);
dayjs.extend(isToday);
dayjs.extend(isSameOrAfter);
dayjs.extend(isSameOrBefore);

import App from '@/app/App.vue';
import { router } from '@/app/router';

import '@/register-service-worker';

import ButtonComponent from '@wjb/vue/component/ButtonComponent.vue';
import FormComponent from '@wjb/vue/component/form/FormComponent.vue';
import FormInputComponent from '@wjb/vue/component/form/FormInputComponent.vue';
import FormSectionComponent from '@wjb/vue/component/form/FormSectionComponent.vue';
import IconComponent from '@wjb/vue/component/IconComponent.vue';
import ViewComponent from '@/component/ViewComponent.vue';

const app = createApp(App);

app.component('ButtonComponent', ButtonComponent);
app.component('FormComponent', FormComponent);
app.component('FormInputComponent', FormInputComponent);
app.component('FormSectionComponent', FormSectionComponent);
app.component('IconComponent', IconComponent);
app.component('ViewComponent', ViewComponent);

app.use(router);
app.mount('#app');
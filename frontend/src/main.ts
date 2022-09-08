import { createApp } from 'vue';

import '@/app/setup/dayjs';
import '@/app/setup/service-worker';
import { components } from '@/app/setup/components';

import App from '@/app/App.vue';
import { router } from '@/app/router';

const app = createApp(App);
app.use(components);
app.use(router);
app.mount('#app');
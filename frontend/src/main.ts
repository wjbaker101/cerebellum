import { createApp } from 'vue';

import '@/setup/dayjs';

import App from '@/App.vue';

import { components } from '@/setup/components';
import { router } from '@/setup/router';

const app = createApp(App);
app.use(components);
app.use(router);
app.mount('#app');
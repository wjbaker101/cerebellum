import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import path from 'path';

import { pwa } from './vite/pwa';

export default defineConfig({

    plugins: [
        vue(),
        pwa(),
    ],

    resolve: {
        alias: [
            {
                find: /^~(.*)$/,
                replacement: '$1',
            },
            {
                find: '@',
                replacement: path.resolve(__dirname, 'src'),
            }
        ],
    },

    server: {
        proxy: {
            '/api': {
                target: 'http://localhost:7468',
            },
        },
    },

});
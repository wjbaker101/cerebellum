import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import path from 'path';

import { pwa } from './src/vite/pwa';

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

    build: {
        outDir: '../backend/Cerebellum/wwwroot',
        emptyOutDir: true,
    },

    server: {
        proxy: {
            '/api': {
                target: 'https://localhost:44309',
                secure: false,
            },
        },
    },

});
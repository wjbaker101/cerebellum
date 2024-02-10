import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import path from 'path';

export default defineConfig({

    plugins: [
        vue(),
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
        outDir: '../backend/Api/wwwroot',
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
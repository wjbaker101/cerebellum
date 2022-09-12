const { defineConfig } = require('@vue/cli-service');

module.exports = defineConfig({

    devServer: {
        proxy: {
            '/api': {
                target: 'https://localhost:44309',
                ws: true,
                changeOrigin: true,
            },
        },
    },

});
const { defineConfig } = require('@vue/cli-service');

module.exports = defineConfig({

    outputDir: '../cerebellum/Api/wwwroot',

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
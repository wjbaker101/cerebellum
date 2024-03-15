import { VitePWA } from 'vite-plugin-pwa';

export const pwa = function () {
    return VitePWA({

        registerType: 'autoUpdate',

        manifest: {
            name: 'Cerebellum',
            short_name: 'Cerebellum',
            description: 'Your mind in digital form.',
            theme_color: '#4db87b',
            icons: [
                {
                    src: 'pwa-192x192.png',
                    sizes: '192x192',
                    type: 'image/png',
                },
                {
                    src: 'pwa-512x512.png',
                    sizes: '512x512',
                    type: 'image/png',
                },
            ],
        },

    });
};
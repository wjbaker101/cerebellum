
import { VitePWA } from 'vite-plugin-pwa';

export const pwa = function () {
    return VitePWA({

        registerType: 'autoUpdate',

        manifest: {
            name: 'Cerebellum',
            short_name: 'Cerebellum',
            description: 'Your mind in digital form.',
            theme_color: '#4db87b',
        },

    });
};
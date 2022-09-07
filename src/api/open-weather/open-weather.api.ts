import { config } from '@/config/secret.config';

import { IGetWeatherResponse } from '@/api/open-weather/type/GetWeather.type';
import { IWeather } from '@/model/Weather.model';

const baseUrl = 'https://api.openweathermap.org/data/2.5';

const mapIcon = function(code: string): string {
    switch (code) {
        case '01d':
            return 'clear-day';
        case '01n':
            return 'clear-night';
        case '02d':
            return 'cloud';
        case '02n':
            return 'loud-night';
        case '03d':
            return 'mist-cloud';
        case '03n':
            return 'mist-cloud';
        case '04d':
            return 'cloud';
        case '04n':
            return 'cloud-night';
        case '09d':
            return 'clear-day';
        case '09n':
            return 'clear-day';
        case '10d':
            return 'clear-day';
        case '10n':
            return 'clear-day';
        case '11d':
            return 'clear-day';
        case '11n':
            return 'clear-day';
        case '13d':
            return 'clear-day';
        case '13n':
            return 'clear-day';
        case '50d':
            return 'mist-day';
        case '50n':
            return 'mist-night';
        default:
            return '';
    }
};

export const openWeatherApi = {

    async getWeather(location: string): Promise<IWeather> {
        const response = await fetch(`${baseUrl}/weather?q=${location}&appid=${config.openWeatherApi.apiKey}`);
        const json = await response.json() as IGetWeatherResponse;

        const weather = json.weather[0];

        return {
            description: weather.main,
            icon: mapIcon(weather.icon),
        };
    },
};
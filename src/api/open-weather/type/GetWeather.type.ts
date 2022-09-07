export interface IGetWeatherResponse {
    coord: ICoord;
    weather: Array<IWeather>;
}

export interface ICoord {
    lon: number;
    lat: number;
}

export interface IWeather {
    main: string;
    description: string;
    icon: string;
}
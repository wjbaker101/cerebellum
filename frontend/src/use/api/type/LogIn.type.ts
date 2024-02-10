export interface ILogInRequest {
    readonly username: string;
    readonly password: string;
}

export interface ILogInResponse {
    readonly loginToken: string;
    readonly user: {
        readonly reference: string;
        readonly createdAt: string;
        readonly username: string;
    };
}
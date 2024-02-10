namespace Core.Settings;

public sealed class AppSecrets
{
    public required DatabaseSettings Database { get; init; }
    public required AuthSettings Auth { get; init; }

    public sealed class DatabaseSettings
    {
        public required string Host { get; init; } 
        public required int Port { get; init; }
        public required string Database { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; } 
    }

    public sealed class AuthSettings
    {
        public required string Pepper { get; init; }
        public required string LoginToken { get; init; }
    }
}
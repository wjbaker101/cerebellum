namespace Core.Settings;

public sealed class AppSecrets
{
    public DatabaseSettings Database { get; init; } = null!;

    public sealed class DatabaseSettings
    {
        public string Host { get; init; } = null!;
        public int Port { get; init; }
        public string Database { get; init; } = null!;
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
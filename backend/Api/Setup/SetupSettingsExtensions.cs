namespace Api.Setup;

public static class SetupSettingsExtensions
{
    public static void SetupSettings(this WebApplicationBuilder builder)
    {
        var isDev = builder.Environment.IsDevelopment();

        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile(GetFile("appsettings", isDev))
            .AddJsonFile(GetFile("appsecrets", isDev));
    }

    private static string GetFile(string file, bool isDev)
    {
        if (isDev)
            return $"{file}.Development.json";

        return $"{file}.json";
    }
}
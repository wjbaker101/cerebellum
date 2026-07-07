namespace Cerebellum.Setup;

public static class SetupSettings
{
    public static void BuildSettings(this WebApplicationBuilder builder)
    {
        var isDev = builder.Environment.IsDevelopment();

        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile(GetFile("appsettings", isDev));
    }

    private static string GetFile(string file, bool isDev)
    {
        if (isDev)
            return $"{file}.Development.json";

        return $"{file}.json";
    }
}
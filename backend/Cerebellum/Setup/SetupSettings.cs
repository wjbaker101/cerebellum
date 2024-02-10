using Core.Settings;

namespace Cerebellum.Setup;

public static class SetupSettings
{
    public static void BuildSettings(this WebApplicationBuilder builder)
    {
        var isDev = builder.Environment.IsDevelopment();

        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile(GetFile("appsettings", isDev))
            .AddJsonFile(GetFile("appsecrets", isDev));

        builder.Services.AddSingleton(builder.Configuration.Get<AppSecrets>()!);
    }

    private static string GetFile(string file, bool isDev)
    {
        if (isDev)
            return $"{file}.Development.json";

        return $"{file}.json";
    }
}
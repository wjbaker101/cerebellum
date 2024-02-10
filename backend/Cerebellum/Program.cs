using Cerebellum.Middleware.Authentication;
using Cerebellum.Setup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.SetupSettings();
builder.SetupDependencies();

services.AddControllers();

services.AddSpaStaticFiles(spa =>
{
    spa.RootPath = "wwwroot";
});

var app = builder.Build();

app.UseMiddleware<AuthenticationMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "wwwroot";
});

app.Run();
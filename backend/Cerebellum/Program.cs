using Cerebellum.Setup;
using Core.Settings;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.SetupSettings();
services.AddSingleton(builder.Configuration.Get<AppSecrets>()!);

services.AddDependencies();
services.AddMiddleware();
services.AddControllers();
services.AddFrontend();

var app = builder.Build();

app.UseMiddleware();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseFrontend();

app.UseDefaultFiles();
app.UseStaticFiles();
app.Run();
using Cerebellum.Setup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.SetupSettings();
builder.SetupDependencies();

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
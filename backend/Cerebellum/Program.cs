using Cerebellum.Setup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.BuildSettings();

services.AddDependencies();
services.AddMiddleware();
services.AddControllers();
services.AddFrontend();

var app = builder.Build();

app.UseMiddleware();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseFrontend();

app.Run();
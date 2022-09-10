using Api.Calendar;
using Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();

services.AddSingleton<ICalendarService, CalendarService>();
services.AddSingleton<ICalendarRepository, CalendarRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
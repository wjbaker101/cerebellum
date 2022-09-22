using Api.Calendar;
using Api.Notes;
using Data;
using Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();

services.AddSingleton<IApiDatabase, ApiDatabase>();

services.AddSingleton<ICalendarService, CalendarService>();
services.AddSingleton<ICalendarRepository, CalendarRepository>();

services.AddSingleton<INotesService, NotesService>();
services.AddSingleton<INotesRepository, NotesRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
using Api;
using Api.Calendar;
using Api.Listum;
using Api.Notes;
using Api.WorkoutDiary;
using Core.Settings;
using Data;
using Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.SetupSettings();

services.AddControllers();

services.AddSingleton(builder.Configuration.Get<AppSecrets>());

services.AddSingleton<IApiDatabase, ApiDatabase>();

services.AddSingleton<ICalendarService, CalendarService>();
services.AddSingleton<ICalendarRepository, CalendarRepository>();

services.AddSingleton<INotesService, NotesService>();
services.AddSingleton<INotesRepository, NotesRepository>();

services.AddSingleton<IListumService, ListumService>();
services.AddSingleton<IListumRepository, ListumRepository>();

services.AddSingleton<IWorkoutDiaryService, WorkoutDiaryService>();
services.AddSingleton<IWorkoutDiaryRepository, WorkoutDiaryRepository>();

services.AddSingleton<IKanbanRepository, KanbanRepository>();

services.AddSpaStaticFiles(spa =>
{
    spa.RootPath = "wwwroot";
});

var app = builder.Build();

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
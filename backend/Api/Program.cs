using Api.Calendar;
using Api.Listum;
using Api.Notes;
using Api.WorkoutDiary;
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

services.AddSingleton<IListumService, ListumService>();
services.AddSingleton<IListumRepository, ListumRepository>();

services.AddSingleton<IWorkoutDiaryService, WorkoutDiaryService>();
services.AddSingleton<IWorkoutDiaryRepository, WorkoutDiaryRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
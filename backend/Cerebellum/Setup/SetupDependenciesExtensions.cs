using Cerebellum.Api.Calendar;
using Cerebellum.Api.Dashboard;
using Cerebellum.Api.Kanban;
using Cerebellum.Api.Listum;
using Cerebellum.Api.Notes;
using Cerebellum.Api.WorkoutDiary;
using Core.Settings;
using Data;
using Data.Repositories;
using Data.Repositories.Dashboard;
using Data.Repositories.WorkoutDiary;

namespace Cerebellum.Setup;

public static class SetupDependenciesExtensions
{
    public static void SetupDependencies(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddSingleton(builder.Configuration.Get<AppSecrets>());

        services.AddSingleton<IApiDatabase, ApiDatabase>();

        services.AddSingleton<ICalendarService, CalendarService>();
        services.AddSingleton<ICalendarRepository, CalendarRepository>();

        services.AddSingleton<IDashboardService, DashboardService>();
        services.AddSingleton<IDashboardRepository, DashboardRepository>();

        services.AddSingleton<INotesService, NotesService>();
        services.AddSingleton<INotesRepository, NotesRepository>();

        services.AddSingleton<IListumService, ListumService>();
        services.AddSingleton<IListumRepository, ListumRepository>();

        services.AddSingleton<IWorkoutDiaryService, WorkoutDiaryService>();
        services.AddSingleton<IWorkoutDiaryRepository, WorkoutDiaryRepository>();

        services.AddSingleton<IKanbanService, KanbanService>();
        services.AddSingleton<IKanbanRepository, KanbanRepository>();
    }
}
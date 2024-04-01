using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.LogWork;
using Service.Phases;
using Service.Telegram;

namespace PT1_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });
        }
        public static void AddUserService(this IServiceCollection services) =>
            services.AddScoped<IUserService, UserService>();
        public static void AddDepartmentService(this IServiceCollection services) =>
            services.AddScoped<IDepartmentService, DepartmentService>();
        public static void AddJobPostionService(this IServiceCollection services) =>
            services.AddScoped<IJobPositionService, JobPositionServive>();
        public static void AddProjectService(this IServiceCollection services) =>
            services.AddScoped<IProjectService, ProjectService>();
        public static void AddTelegramService(this IServiceCollection services) =>
            services.AddScoped<ITelegramService, TelegramService>();
        public static void AddPhaseService(this IServiceCollection services) =>
            services.AddScoped<IPhaseService, PhaseService>();
        public static void AddLevelService(this IServiceCollection services) =>
            services.AddScoped<ICommonCodeService, LevelService>();
        public static void AddLogWorkService(this IServiceCollection services) =>
            services.AddScoped<ILogWorkService, LogWorkService>();

    }
}

using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Mapper;
using PsySchedule.Middlewares;
using PsySchedule.Models;
using PsySchedule.Services;

namespace PsySchedule.Depends
{
    public static class Depends
    {
        public static WebApplicationBuilder UseDepends(this WebApplicationBuilder builder)
        {
            //IOptions
            builder.Services.Configure<TokenParameters>(builder.Configuration.GetSection("Jwt"));

            builder.Services.AddTransient<UseExceptionHandler>();

            builder.Services.AddScoped<IMapper<ScheduleTemplateDayDto,ScheduleTemplate,int>, ScheduleTemplateMapper>();
            builder.Services.AddScoped<IMapper<ServiceResponse, Service>, ServiceMapper>();

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddScoped<IScheduleTemplateService, ScheduleTemplateService>();
            builder.Services.AddScoped<IDayService, DayService>();

            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            return builder;
        }
    }
}

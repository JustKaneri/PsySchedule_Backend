using FluentValidation;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Mapper;
using PsySchedule.Middlewares;
using PsySchedule.Models;
using PsySchedule.Services;
using PsySchedule.Validations;

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
            builder.Services.AddScoped<IMapper<GRPC.ClientTelegramRequest, Client>, ClientTelegramMapper>();

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddScoped<IScheduleTemplateService, ScheduleTemplateService>();
            builder.Services.AddScoped<IDayService, DayService>();

            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            builder.Services.AddScoped<IVacationService, VacationService>();

            builder.Services.AddScoped<IClientService<GRPC.ClientTelegramRequest>, ClientTelegramService>();


            builder.Services.AddScoped<IValidator<AuthenticationDto>, AuthenticationValidator>();
            builder.Services.AddScoped<IValidator<GRPC.ClientTelegramRequest>, ClientTelegramRequestValidator>();
            builder.Services.AddScoped<IValidator<CreateVacationRequest>, CreateVacationRequestValidator>();
            builder.Services.AddScoped<IValidator<RegisterPsychologistDto>, PsychologistRegistrationValidator>();
            builder.Services.AddScoped<IValidator<ScheduleTemplateDayDto>, ScheduleTemplateDayDtoValidator>();
            builder.Services.AddScoped<IValidator<IEnumerable<ScheduleTemplateDayDto>>, ScheduleTemplatesValidator>();
            builder.Services.AddScoped<IValidator<ServiceDto>, ServiceDtoValidator>();
            builder.Services.AddScoped<IValidator<GRPC.TelegramData>, TelegramDataValidator>();
            builder.Services.AddScoped<IValidator<TimeRange>, TimeRangeValidator>();
            builder.Services.AddScoped<IValidator<UpdateServiceRequest>, UpdateServiceRequestValidator>();

            builder.Services.AddScoped<Interfaces.IValidatorFactory, ValidatorFactory>();


            return builder;
        }
    }
}

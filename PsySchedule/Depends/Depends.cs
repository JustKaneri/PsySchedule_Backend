using PsySchedule.Interfaces;
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

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            return builder;
        }
    }
}

using PsySchedule.Interfaces;
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

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

            return builder;
        }
    }
}

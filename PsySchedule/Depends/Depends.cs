using PsySchedule.Models;

namespace PsySchedule.Depends
{
    public static class Depends
    {
        public static WebApplicationBuilder UseDepends(this WebApplicationBuilder builder)
        {
            //IOptions
            builder.Services.Configure<JwtParameters>(builder.Configuration.GetSection("Jwt"));

            return builder;
        }
    }
}

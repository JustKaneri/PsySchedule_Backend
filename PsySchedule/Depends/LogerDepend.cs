using Serilog;

namespace PsySchedule.Depends
{
    public static class LogerDepend
    {
        public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
        {
            Log.Logger = Log.Logger = new LoggerConfiguration()
                          .ReadFrom.Configuration(builder.Configuration)
                          .Enrich.FromLogContext()
                          .CreateLogger();

            builder.Host.UseSerilog();

            return builder;
        }
    }
}

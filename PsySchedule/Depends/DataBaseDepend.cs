using Microsoft.EntityFrameworkCore;
using PsySchedule.Context;

namespace PsySchedule.Depends
{
    public static class DataBaseDepend
    {
        public static WebApplicationBuilder UsePgSQl(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("PsySchedule"));
            });

            return builder;
        }
    }
}

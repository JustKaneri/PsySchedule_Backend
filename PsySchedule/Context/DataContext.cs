using Microsoft.EntityFrameworkCore;

namespace PsySchedule.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) { }
    }
}

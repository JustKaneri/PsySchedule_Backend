using Microsoft.EntityFrameworkCore;
using PsySchedule.Models;

namespace PsySchedule.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }   
        public DbSet<AppointmentStatus> AppointmentStatuses { get; set; }   
        public DbSet<AppointmentНistory> AppointmentНistories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Psychologist> Psychologists  { get; set; }
        public DbSet<ScheduleTemplate> ScheduleTemplates { get; set; }
        public DbSet<Service> Services { get; set; } 
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }

        public DataContext(DbContextOptions<DataContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Psychologist>()
                        .HasOne<ScheduleTemplate>(psy => psy.ScheduleTemplate)
                        .WithOne(st => st.Psychologist)
                        .HasForeignKey<Psychologist>(ps => ps.ScheduleTemplateId);

            modelBuilder.Entity<Client>().Property(c => c.Rating).HasDefaultValue(1);
            modelBuilder.Entity<ScheduleTemplate>().Property(st => st.Gap).HasDefaultValue(15);
            modelBuilder.Entity<Appointment>().Property(st => st.IsConfirmationClient).HasDefaultValue(false);
            modelBuilder.Entity<Appointment>().Property(st => st.IsConfirmationPsychologist).HasDefaultValue(false);

        }

    }
}

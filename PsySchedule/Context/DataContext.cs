using Microsoft.EntityFrameworkCore;
using PsySchedule.Models;
using PsySchedule.Models.Enums;

namespace PsySchedule.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }   
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

            modelBuilder.Entity<ScheduleTemplate>().Property(st => st.Weekday).HasConversion<string>();
            modelBuilder.Entity<ScheduleTemplate>().Property(st => st.Gap).HasDefaultValue(15);
            modelBuilder.Entity<ScheduleTemplate>().HasIndex(st => new { st.PsychologistId, st.Weekday }).IsUnique();

            modelBuilder.Entity<Client>().Property(c => c.Rating).HasDefaultValue(1);

            modelBuilder.Entity<Appointment>().Property(st => st.ClientConfirmation)
                                              .HasConversion<string>()
                                              .HasDefaultValue(ConfirmationStatus.Pending);


            modelBuilder.Entity<Appointment>().Property(st => st.PsychologistConfirmation)
                                              .HasConversion<string>()
                                              .HasDefaultValue(ConfirmationStatus.Pending);

            modelBuilder.Entity<Appointment>().Property(st => st.Status)
                                              .HasConversion<string>()
                                              .HasDefaultValue(AppointmentStatus.Created);


            modelBuilder.Entity<WorkDay>().Property(wd => wd.State)
                                          .HasConversion<string>()
                                          .HasDefaultValue(WorkDayState.Generated);

            modelBuilder.Entity<WorkDay>().HasIndex(wd => new { wd.Date, wd.PsychologistId }).IsUnique();
        }
    }
}

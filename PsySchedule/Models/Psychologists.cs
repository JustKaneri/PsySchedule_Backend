using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("User")]
    public class Psychologists
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public DateTime DateRegistryUtc { get; set; } = DateTime.UtcNow;

        public IEnumerable<Vacations> Vacations { get; set; }

        public IEnumerable<RefreshToken> RefreshTokens { get; set; }

        public int ScheduleTemplateId { get;set;  }

        public ScheduleTemplate ScheduleTemplate { get; set; }

        public IEnumerable<WorkDay> WorkDays { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }    

        public IEnumerable<Service> Services { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("Psychologist")]
    public class Psychologist
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Login { get; set; }

        public required string Password { get; set; }

        public required string Salt { get; set; }

        public string TimeZone { get; set; }

        public DateTimeOffset RegisteredAt { get; set; } = DateTime.UtcNow;

        public IEnumerable<Vacations> Vacations { get; set; }

        public IEnumerable<Token> RefreshTokens { get; set; }

        public int ScheduleTemplateId { get;set;  }

        public ScheduleTemplate ScheduleTemplate { get; set; }

        public IEnumerable<WorkDay> WorkDays { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }    

        public IEnumerable<Service> Services { get; set; }
    }
}

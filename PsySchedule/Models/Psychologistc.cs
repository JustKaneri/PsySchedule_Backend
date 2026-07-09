using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("Psychologist")]
    [Index("Login")]
    public class Psychologist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Login { get; set; }

        public required string Password { get; set; }

        public required string Salt { get; set; }

        public string TimeZone { get; set; }

        public DateTimeOffset RegisteredAt { get; set; } = DateTimeOffset.UtcNow;

        public IEnumerable<Vacation> Vacations { get; set; }

        public IEnumerable<Token> RefreshTokens { get; set; }

        public Nullable<int> ScheduleTemplateId { get;set;  }

        public ScheduleTemplate? ScheduleTemplate { get; set; }

        public IEnumerable<WorkDay> WorkDays { get; set; }

        public IEnumerable<Appointment> Appointments { get; set; }     

        public IEnumerable<Service> Services { get; set; }
    }
}

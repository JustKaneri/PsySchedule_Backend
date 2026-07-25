using Microsoft.EntityFrameworkCore;
using PsySchedule.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    /// <summary>
    /// Запись
    /// </summary>
    [Table("Appointment")]
    [Index("StartedAt")]
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PsychologistId { get; set; }

        public Psychologist Psychologist { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }  

        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset FinishedAt { get; set; }

        public AppointmentStatus Status { get; set; }

        public ConfirmationStatus ClientConfirmation { get; set; }

        public ConfirmationStatus PsychologistConfirmation { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public int ServiceId { get; set; }

        public Service Service { get; set; }

        public int WorkDayId { get; set; }

        public WorkDay WorkDay { get; set; }

        public IEnumerable<AppointmentНistory> AppointmentНistories { get; set; }   

    }
}

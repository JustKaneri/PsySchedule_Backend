using PsySchedule.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    /// <summary>
    /// История изменения статуса записи
    /// </summary>
    [Table("AppointmentНistory")]
    public class AppointmentНistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public AppointmentStatus OldState { get; set; }

        public AppointmentStatus NewState { get; set; }
    }
}

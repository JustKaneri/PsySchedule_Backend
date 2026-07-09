using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("AppointmentНistory")]
    public class AppointmentНistory
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }  
        
        public DateTimeOffset CreatedAt { get; set; }

        public int OldState { get; set; }

        public int NewState { get; set; }
    }
}

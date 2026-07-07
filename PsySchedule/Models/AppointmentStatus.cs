using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("AppointmentStatus")]
    public class AppointmentStatus
    {
        public int Id { get; set; } 

        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("Vacations")]
    public class Vacations
    {
        public int Id { get; set; } 

        public DateTime StartTime { get; set;  }

        public DateTime EndTime { get; set; }

        public int PsychologistsId { get;set;  }

        public Psychologists Psychologists { get; set; }
    }
}

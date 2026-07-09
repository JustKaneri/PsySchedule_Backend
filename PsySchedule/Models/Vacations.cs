using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("Vacations")]
    public class Vacations
    {
        public int Id { get; set; } 

        public DateOnly StartedAt { get; set;  }

        public DateOnly FinishedAt { get; set; }

        public int PsychologistId { get;set;  }

        public Psychologist Psychologist { get; set; }
    }
}

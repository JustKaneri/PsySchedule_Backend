using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    /// <summary>
    /// Период отпуска психолога 
    /// </summary>
    [Table("Vacation")]
    public class Vacation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        public DateOnly StartedAt { get; set;  }

        public DateOnly FinishedAt { get; set; }

        public int PsychologistId { get;set;  }

        public Psychologist Psychologist { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    /// <summary>
    /// Шаблон расписания
    /// </summary>
    [Table("ScheduleTemplate")]
    [Index("PsychologistId")]
    public class ScheduleTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Weekend { get; set; }

        public TimeOnly StartedAt { get; set; }

        public TimeOnly FinishedAt { get; set; }

        public TimeOnly BreakStartedAt { get; set; }

        public TimeOnly BreakFinishedAt { get; set; }

        /// <summary>
        /// Пауза между сессиями в минутах.
        /// </summary>
        public int Gap { get; set; } = 15;

        public int PsychologistId { get; set; }

        public Psychologist Psychologist { get; set; }
    }
}
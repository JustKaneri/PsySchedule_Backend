using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("ScheduleTemplate")]
    public class ScheduleTemplate
    {
        public int Id { get; set; }

        public string WeekEnd { get; set; }

        public TimeOnly StartedAt { get; set; }

        public TimeOnly FinishedAt { get; set; }

        public TimeOnly BreakStartedAt { get; set; }

        public TimeOnly BreakFinishedAt { get; set; }

        /// <summary>
        /// Пауза между сессиями в минутах.
        /// </summary>
        public int Gap { get; set; }

        public int PsychologistsId { get; set; }

        public Psychologist Psychologist { get; set; }
    }
}
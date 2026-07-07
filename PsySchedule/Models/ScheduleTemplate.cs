using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("ScheduleTemplate")]
    public class ScheduleTemplate
    {
        public int Id { get; set; }

        public string WeekEnd { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public TimeOnly BreakStart { get; set; }

        public TimeOnly BreakEnd { get; set; }

        public int PsychologistsId { get; set; }

        public Psychologists Psychologists { get; set; }
    }
}
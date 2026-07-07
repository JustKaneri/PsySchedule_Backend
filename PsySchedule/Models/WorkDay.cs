using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("WorkDay")]
    public class WorkDay
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeOnly BreakStart { get; set; }

        public TimeOnly BreakEnd { get; set; }

        public int ScheduleTemplateId { get; set; }

        public ScheduleTemplate ScheduleTemplate { get; set; }  

        public int PsychologistsId { get; set; }

        public Psychologists Psychologists { get; set; } 

        public string Status { get; set; }  
    }
}
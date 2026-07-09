using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("WorkDay")]
    public class WorkDay
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }  

        public TimeOnly StartedAt { get; set; }

        public TimeOnly FinishedAt { get; set; }

        public TimeOnly BreakStartedAt { get; set; }

        public TimeOnly BreakFinishedAt { get; set; }

        public int ScheduleTemplateId { get; set; }

        public ScheduleTemplate ScheduleTemplate { get; set; }  

        public int PsychologistId { get; set; }

        public Psychologist Psychologist { get; set; } 

        public string Status { get; set; }  
    }
}
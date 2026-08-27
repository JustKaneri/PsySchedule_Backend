using Microsoft.EntityFrameworkCore;
using PsySchedule.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    /// <summary>
    /// Рабочий день
    /// </summary>
    [Table("WorkDay")]
    [Index("Date")]
    public class WorkDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public WeekDay Weekday { get; set; }

        public DateOnly Date { get; set; }  

        public TimeOnly StartedAt { get; set; }

        public TimeOnly FinishedAt { get; set; }

        public TimeOnly BreakStartedAt { get; set; }

        public TimeOnly BreakFinishedAt { get; set; }

        public int Gap { get; set; }

        public int ScheduleTemplateId { get; set; }

        public ScheduleTemplate ScheduleTemplate { get; set; }  

        public int PsychologistId { get; set; }

        public Psychologist Psychologist { get; set; } 

        public WorkDayState State { get; set; }  

        public IEnumerable<Appointment> Appointments { get; set; }
    }
}
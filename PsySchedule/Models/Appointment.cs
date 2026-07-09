using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("Appointment")]
    public class Appointment
    {
        public int Id { get; set; }

        public int PsychologistId { get; set; }

        public Psychologist Psychologist { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }  

        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset FinishedAt { get; set; }

        public int StatusId { get; set; }

        public AppointmentStatus Status { get; set; }

        public bool IsConfirmationClient { get; set; }

        public bool IsConfirmationPsychologist { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public int ServiceId { get; set; }

        public Service Service { get; set; }

        public int WorkDayId { get; set; }

        public WorkDay WorkDay { get; set; }

    }
}

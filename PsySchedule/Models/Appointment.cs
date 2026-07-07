using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("Appointment")]
    public class Appointment
    {
        public int Id { get; set; }

        public int PsychologistId { get; set; }

        public Psychologists Psychologist { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }  

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int StatusId { get; set; }

        public AppointmentStatus Status { get; set; }

        public bool IsConfirmationClient { get; set; }

        public bool IsConfirmationPsychologist { get; set; }

        public DateTime DateCreate { get; set; }

        public int ServiceId { get; set; }

        public Service Service { get; set; }

        public int WorkDayId { get; set; }

        public WorkDay WorkDay { get; set; }

    }
}

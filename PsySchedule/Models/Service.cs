using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("Service")]
    public class Service
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public int PsychologistId { get; set; }

        public Psychologist Psychologist { get; set; }
    }
}

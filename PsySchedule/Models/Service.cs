using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    /// <summary>
    /// Услуги предоставляемые психологом
    /// </summary>
    [Table("Service")]
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Name { get; set; }

        [Range(0, 500_000)]
        public decimal Price { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public int Version { get; set; }

        public DateTimeOffset? UpdateAt { get; set; }

        public int PsychologistId { get; set; }

        public Psychologist Psychologist { get; set; }

        public List<Appointment> Appointments { get; set; } 
    }
}

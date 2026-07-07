using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("Service")]
    public class Service
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime DateCreate { get; set; }

        public int PsychologistsId { get; set; }

        public Psychologists Psychologists { get; set; }
    }
}

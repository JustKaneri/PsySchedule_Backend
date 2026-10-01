using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    /// <summary>
    /// Клиент
    /// </summary>
    [Table("Client")]
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string? SecondName { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }

        [Range(0,1)]
        public double Rating { get; set; } = 1;

        public string TimeZone { get; set; }

        public int? TelegramId { get; set; }

        public ClientTelegram? Telegram { get; set; }

        public DateTimeOffset RegisteredAt { get; set; } = DateTimeOffset.UtcNow;

        public IEnumerable<Appointment> Appointments {  get; set; }
    }

}

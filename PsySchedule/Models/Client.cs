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

        public long TelegramId { get; set; }

        public string? TelegramName { get; set; }

        public long TelegramChatId { get; set; }

        public string TimeZone { get; set; }

        public DateTimeOffset RegisteredAt { get; set; } = DateTimeOffset.UtcNow;

        public IEnumerable<Appointment> Appointments {  get; set; }
    }

}

using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("")]
    public class Client
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Phone { get; set; }

        public double Rating { get; set; }

        public long TelegramId { get; set; }

        public string TelegramName { get; set; }

        public long TelegramChatId { get; set; }

        public string TimeZone { get; set; }

        public DateTimeOffset RegisteredAt { get; set; } = DateTime.UtcNow;
    }

}

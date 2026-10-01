using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{

    [Table("ClientTelegram")]
    public class ClientTelegram
    {
        [Key]
        [Column("TelegramId")]
        public long Id { get; set; }

        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LasttName { get; set; }

        public string? LanguageCode { get; set; }

        public long TelegramChatId { get; set; }
    }
}

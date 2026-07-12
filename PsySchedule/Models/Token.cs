using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    /// <summary>
    /// Токены авторизации
    /// </summary>
    [Table("Token")]
    [Index("TokenRefresh")]
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PsychologistId { get; set; }

        public Psychologist Psychologist { get; set; }    

        public string TokenRefresh { get; set; }

        public string TokenAccess { get; set; }

        public string UserAgent { get; set; }

        public string Ip { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset ExpiresAt { get; set; }
    }
}

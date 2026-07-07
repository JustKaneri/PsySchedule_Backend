using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsySchedule.Models
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        public int Id { get; set; }

        public int PsychologistsId { get; set; }

        public Psychologists Psychologists { get; set; }    

        public string TokenRefresh { get; set; }

        public string TokenAccess { get; set; }

        public string UserAgent { get; set; }

        public string Ip { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime AddeDate { get; set; } = DateTime.UtcNow;

        public DateTime ExpireDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PsySchedule.Models.Enums
{
    /// <summary>
    /// Статуст подтверждения
    /// </summary>
    public enum ConfirmationStatus
    {
        [Display(Name = "Ожидается подтверждение")]
        Pending,

        [Display(Name = "Подтверждена")]
        Confirmed,

        [Display(Name = "Отклонена")]
        Rejected
    }
}

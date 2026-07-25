using System.ComponentModel.DataAnnotations;

namespace PsySchedule.Models.Enums
{
    /// <summary>
    /// Статус заявки
    /// </summary>
    public enum AppointmentStatus
    {
        [Display(Name = "Создана")]
        Created = 0,

        [Display(Name = "Подтверждена")]
        Confirmed = 1,

        [Display(Name = "Отменена")]
        Cancelled = 2,

        [Display(Name = "Завершена")]
        Completed = 3,

        [Display(Name = "Не явился")]
        NoShow = 4
    }
}

using PsySchedule.Dto;
using System.Reflection.Metadata;

namespace PsySchedule.Interfaces
{
    /// <summary>
    /// Сервис для работы с TimeZone
    /// </summary>
    public interface ITimeZoneService
    {
        /// <summary>
        /// Получить список всех часовых поясов
        /// с текущим UTC-смещением.
        /// </summary>
        public IReadOnlyCollection<TimeZoneInfoDto> GetTimeZones();

        /// <summary>
        /// Проверить существование часового пояса.
        /// </summary>
        public bool CheckIsExists(string timeZoneId);

        /// <summary>
        /// Конвертировать UTC время в локальное
        /// время указанного часового пояса.
        /// </summary>
        public DateTimeOffset ConvertFromUtc(DateTimeOffset utc, string timeZoneId);

        /// <summary>
        /// Конвертировать в UTC время из локального
        /// </summary>
        public DateTimeOffset ConvertToUtc(DateTimeOffset dateTime, string timeZoneId);
    }
}

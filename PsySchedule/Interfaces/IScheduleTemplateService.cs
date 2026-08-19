using PsySchedule.Dto;
using PsySchedule.Models;
using System.Collections;

namespace PsySchedule.Interfaces
{
    /// <summary>
    /// Сервис для работы с шаблонами расписания
    /// </summary>
    public interface IScheduleTemplateService
    {
        /// <summary>
        /// Создание шаблона
        /// </summary>
        /// <param name="scheduleTemplates">Список дней</param>
        /// <param name="psychologistId">Id специалиста</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<Result> CreateAsync(IEnumerable<ScheduleTemplateDayDto> scheduleTemplates, int psychologistId, CancellationToken cancellationToken);

        /// <summary>
        /// Обновление шаблона
        /// </summary>
        /// <param name="scheduleTemplate">Новвые данные</param>
        /// <param name="psychologistId">Id специалиста</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<Result> UpdateOrCreateAsync(ScheduleTemplateDayDto scheduleTemplate, int psychologistId, CancellationToken cancellationToken);

        /// <summary>
        /// Получить шаблон рассписания
        /// </summary>
        /// <param name="psychologistId">Id специалиста</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>IEnumerable ScheduleTemplateDto</returns>
        public Task<Result<ScheduleTemplatesDto>> GetAsync(int psychologistId, CancellationToken cancellationToken);
    }
}

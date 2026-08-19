using PsySchedule.Dto;
using PsySchedule.Models;

namespace PsySchedule.Interfaces
{
    public interface IDayService
    {

        /// <summary>
        /// Добавляет дни расписания, созданные на основе вновь созданного шаблона.
        /// </summary>
        /// <param name="templates">Шалбон</param>
        /// <param name="from">Дата начала</param>
        /// <param name="to">Дата окончания</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public void AddDaysFromTemplate(IEnumerable<ScheduleTemplate> templates, DateOnly from, DateOnly to, CancellationToken cancellationToken);

        /// <summary>
        /// Генерирует расписание на следующий день. 
        /// Используется фоновым работником.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<Result> GenerateNextDayAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Вручную обновляет существующий день расписания.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Result> UpdateDayAsync(ScheduleDayDto dto, CancellationToken cancellationToken);
    }
}

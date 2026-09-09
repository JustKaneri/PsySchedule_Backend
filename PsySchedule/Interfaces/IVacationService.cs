using PsySchedule.Dto;
using PsySchedule.Models;

namespace PsySchedule.Interfaces
{
    /// <summary>
    /// Сервис для работы с отпуском
    /// </summary>
    public interface IVacationService
    {
        /// <summary>
        /// Получить отпуска
        /// </summary>
        /// <param name="psyId">Id психолога</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список отпусков</returns>
        public Task<Result<IEnumerable<VacationResponse>>> GetAsync(int psyId, CancellationToken cancellationToken);

        /// <summary>
        /// Создать новый отпуск
        /// </summary>
        /// <param name="psyId">Id психолога</param>
        /// <param name="request">Данные</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<Result> CreateAsync(int psyId,CreateVacationRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Отмена отпуска
        /// </summary>
        /// <param name="psyId">Id психолога</param>
        /// <param name="vacationId">Id отпуска</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<Result> CancelAsync(int psyId, int vacationId, CancellationToken cancellationToken);
    }
}

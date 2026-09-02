using PsySchedule.Dto;
using PsySchedule.Models;

namespace PsySchedule.Interfaces
{
    /// <summary>
    /// Сервис для работы с услугами
    /// </summary>
    public interface IServiceManager
    {
        /// <summary>
        /// Получить список услуг
        /// </summary>
        /// <param name="psyId">Id психолога</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список услуг</returns>
        public Task<Result<IEnumerable<ServiceResponse>>> GetServicesAsync(int psyId, CancellationToken cancellationToken);

        /// <summary>
        /// Создать новую услугу
        /// </summary>
        /// <param name="psyId">Id психолога</param>
        /// <param name="dataRequest">Данные услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<Result> CreateAsync(int psyId, CreateServiceRequest dataRequest,CancellationToken cancellationToken);

        /// <summary>
        /// Удалить услугу
        /// </summary>
        /// <param name="psyId">Id психолога</param>
        /// <param name="serviceId">Id услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<Result> DeleteAsync(int psyId, int serviceId ,CancellationToken cancellationToken);

        /// <summary>
        /// Обновиь услугу
        /// </summary>
        /// <param name="psyId">Id психолога</param>
        /// <param name="dataRequest">Данные услуги</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<Result> UpdateAsync(int psyId, UpdateServiceRequest dataRequest,CancellationToken cancellationToken);
    }
}

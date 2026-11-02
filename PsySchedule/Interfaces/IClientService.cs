using PsySchedule.Dto;
using PsySchedule.Models;

namespace PsySchedule.Interfaces
{
    /// <summary>
    /// Сервис для регистрации клиента
    /// </summary>
    public interface IClientService<RequestType>
    {
        public Task<Result> CreateAsync(RequestType request, CancellationToken cancellationToken);
    }
}

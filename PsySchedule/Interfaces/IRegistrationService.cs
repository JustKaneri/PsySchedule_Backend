using PsySchedule.Dto;
using PsySchedule.Models;

namespace PsySchedule.Interfaces
{
    public interface IRegistrationService
    {
        /// <summary>
        /// Регистрирует нового психолога.
        /// </summary>
        /// <param name="registerDto">
        /// Данные для регистрации пользователя.
        /// </param>
        /// <param name="cancellationToken">
        /// Токен отмены операции.
        /// </param>
        /// <returns>
        /// Результат регистрации с данными аутентификации.
        /// </returns>
        public Task<Result<AuthTokensDto>> RegisterPsychologistAsync(RegisterPsychologistDto registerDto, MetaDataDto userData, CancellationToken cancellationToken);
    }
}

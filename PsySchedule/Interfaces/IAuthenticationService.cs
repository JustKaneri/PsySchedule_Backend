using PsySchedule.Dto;
using PsySchedule.Models;

namespace PsySchedule.Interfaces
{
    /// <summary>
    /// Сервис аутентификации
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="authenticationData">Данные для аутентификации</param>
        /// <returns>Acsess и Refresh токены</returns>
        public Task<Result<AuthTokensDto>> AuthenticateAsync(AuthenticationDto authenticationData, UserDataDto userData, CancellationToken cancellationToken);

        /// <summary>
        /// Обновление токенов
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Result<AuthTokensDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

        /// <summary>
        /// Завершение пользовательской сесии
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Result<bool>> LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    }
}

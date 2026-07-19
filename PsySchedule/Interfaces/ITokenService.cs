using PsySchedule.Dto;
using PsySchedule.Models;

namespace PsySchedule.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Создание токена
        /// </summary>
        /// <returns></returns>
        public Token CreateToken(int userId, UserDataDto userData);

        /// <summary>
        /// Верификация Refresh токена и Access
        /// </summary>
        /// <returns></returns>
        public Boolean VerificationToken();
    }
}

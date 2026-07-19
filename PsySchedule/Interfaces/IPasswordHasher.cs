namespace PsySchedule.Interfaces
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Хэширование пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="Salt">Соль для хэширования</param>
        /// <returns>Хэш пароля</returns>
        public string Hash(string password, string salt);
        
        /// <summary>
        /// Сравнение хэшей паролей
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="passwordHash">Хэш пароля</param>
        /// <param name="salt">Соль для хэширования</param>
        /// <returns>True при совпадение и False в ином случае</returns>
        public bool Verify(string password, string passwordHash,string salt);
    }
}

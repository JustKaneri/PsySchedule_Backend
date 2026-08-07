namespace PsySchedule.Dto
{
    /// <summary>
    /// Данные для аутентификация
    /// </summary>
    /// <param name="Login">Логин, от 6 до 30 символов</param>
    /// <param name="Password">Пароль, от 8 до 32 символов</param>
    public record AuthenticationDto(string Login,string Password);

    /// <summary>
    /// Данные для регистрации
    /// </summary>
    /// <param name="Name">Имя, от 6 до 50 символов</param>
    /// <param name="Login">Логин, от 6 до 30 символов</param>
    /// <param name="Password">Пароль, от 8 до 32 символов</param>
    /// <param name="TimeZone">Часовой пояс</param>
    public record RegisterPsychologistDto(string Name, string Login,string Password, string TimeZone);

    public record MetaDataDto(string Ip, string UserAgent);
}

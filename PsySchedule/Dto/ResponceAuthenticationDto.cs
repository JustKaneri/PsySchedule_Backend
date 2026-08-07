namespace PsySchedule.Dto
{
    /// <summary>
    /// Responce. 
    /// AccessToken и RefreshToken
    /// </summary>
    /// <param name="AccessToken">Токен для доступа</param>
    /// <param name="RefreshToken">Токен для получения нового Access токена</param>
    public record AuthTokensDto(string AccessToken, string RefreshToken);

    /// <summary>
    /// Access токен
    /// </summary>
    /// <param name="Token"></param>
    public record struct AccessTokenDto(string Token);
}

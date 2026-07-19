namespace PsySchedule.Dto
{
    public record AuthTokensDto(string AccessToken, string RefreshToken);

    public record struct AccessTokenDto(string Token);
}

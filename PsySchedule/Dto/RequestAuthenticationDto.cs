namespace PsySchedule.Dto
{
    public record AuthenticationDto(string Login,string Password);

    public record RegisterPsychologistDto(string Name, string Login,string Password, string TimeZone);

    public record MetaDataDto(string Ip, string UserAgent);
}

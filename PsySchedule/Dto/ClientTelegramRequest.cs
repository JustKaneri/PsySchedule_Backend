using System.ComponentModel.DataAnnotations;

namespace PsySchedule.Dto
{
    public record ClientTelegramRequest(string FirstName, string? SecondName, string? Phone,  string TimeZone, TelegramData TelegramData);

    public record TelegramData(long Id, string? UserName, string? FirstName, string? LasttName, string? LanguageCode, long TelegramChatId);
}

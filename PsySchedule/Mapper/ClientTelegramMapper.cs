using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;

namespace PsySchedule.Mapper
{
    public class ClientTelegramMapper : IMapper<GRPC.ClientTelegramRequest, Client>
    {
        public Client FromDto(GRPC.ClientTelegramRequest dto)
        {
            return new Client()
            {
                FirstName = dto.FirstName,
                SecondName = dto.SecondName,
                Phone = dto.Phone,
                TimeZone = dto.TimeZone,
                TelegramId = dto.TelegramData.Id,
                Telegram = new ClientTelegram
                {
                    Id = dto.TelegramData.Id,
                    FirstName = dto.TelegramData.FirstName,
                    UserName = dto.TelegramData.UserName,
                    LasttName = dto.TelegramData.LastName,
                    LanguageCode = dto.TelegramData.LanguageCode,
                    TelegramChatId = dto.TelegramData.TelegramChatId
                }
            };
        }

        public GRPC.ClientTelegramRequest FromEntity(Client entity)
        {
            throw new NotImplementedException();
        }
    }
}

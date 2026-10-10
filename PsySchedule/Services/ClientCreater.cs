using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;

namespace PsySchedule.Services
{
    public class ClientCreater : IClientCreater
    {
        public Task<Result> CreateAsync(ClientTelegramRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

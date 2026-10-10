using PsySchedule.Dto;
using PsySchedule.Models;

namespace PsySchedule.Interfaces
{
    public interface IClientCreater
    {
        public Task<Result> CreateAsync(ClientTelegramRequest request, CancellationToken cancellationToken);
    }
}

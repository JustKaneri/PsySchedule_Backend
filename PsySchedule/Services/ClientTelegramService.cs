using Microsoft.EntityFrameworkCore;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Health;
using PsySchedule.Interfaces;
using PsySchedule.Models;

namespace PsySchedule.Services
{
    /// <summary>
    /// Сервис для регистрации клиента через телеграмм
    /// </summary>
    public class ClientTelegramService : IClientService<GRPC.ClientTelegramRequest>
    {
        private readonly DataContext _context;
        private readonly ILogger<ClientTelegramService> _logger;
        private readonly IMapper<GRPC.ClientTelegramRequest, Client> _mapper;

        public ClientTelegramService(DataContext context, ILogger<ClientTelegramService> logger, IMapper<GRPC.ClientTelegramRequest, Client> mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> CreateAsync(GRPC.ClientTelegramRequest request, CancellationToken cancellationToken)
        {
            var isExist = await _context.Clients.AnyAsync(c => c.TelegramId == request.TelegramData.Id,cancellationToken);

            if (isExist)
            {
                _logger.LogWarning($"Client registration skipped because a client with TelegramId {request.TelegramData.Id} already exists.");
                return Result.Success();
            }

            _context.Add(_mapper.FromDto(request));

            await _context.SaveChangesAsync(cancellationToken);

            HealthService.RegistrationsTelegramCountMetric.Inc();

            return Result.Success();

        }
    }
}

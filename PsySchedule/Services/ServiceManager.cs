using Microsoft.EntityFrameworkCore;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;

namespace PsySchedule.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly DataContext _context;
        private readonly ILogger<ServiceManager> _logger;
        private readonly IMapper<ServiceResponse, Service> _mapper;

        public ServiceManager(DataContext context, ILogger<ServiceManager> logger, IMapper<ServiceResponse, Service> mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Result> CreateAsync(int psyId, CreateServiceRequest dataRequest, CancellationToken cancellationToken)
        {
            var exist = await _context.Psychologists.AsNoTracking().AnyAsync(ps => ps.Id == psyId,cancellationToken);

            if(!exist)
            {
                _logger.LogWarning("Failed to create service. Psychologist {PsychologistId} was not found", psyId);

                return Result.Failure(404, "Психолог не найден");
            }

            var service = new Service() { Name = dataRequest.Name, Price = dataRequest.Price, PsychologistId = psyId, Version = 1 };

            _context.Services.Add(service);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int psyId, int serviceId, CancellationToken cancellationToken)
        {
            var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == serviceId, cancellationToken);

            if(service == null)
            {
                _logger.LogWarning("Failed to delete service {ServiceId}: service was not found", serviceId);

                return Result.Failure( 404, $"Услуга {serviceId} не найдена");
            }

            if(service.PsychologistId != psyId)
            {
                _logger.LogWarning("Access denied. Psychologist {PsychologistId} attempted to delete " +
                                    "service {ServiceId} owned by psychologist {OwnerPsychologistId}", psyId,serviceId,service.PsychologistId);

                return Result.Failure(403, "У вас нет прав для удаления данной услуги");
            }

            var rowDeleted = await _context.Services.Where(s => s.Id == serviceId).ExecuteDeleteAsync(cancellationToken);

            if(rowDeleted == 0)
            {
                _logger.LogWarning("Failed to delete service {ServiceId}: service was already deleted or changed", serviceId);

                return Result.Failure( 409, "Не удалось удалить услугу. Возможно, она уже была удалена");
            }

            return Result.Success();
        }

        public async Task<Result<IEnumerable<ServiceResponse>>> GetServicesAsync(int psyId, CancellationToken cancellationToken)
        {
            var services = await _context.Services.Where(s => s.PsychologistId == psyId).ToListAsync(cancellationToken);

            if (services.Count == 0)
            {
                _logger.LogInformation("No services found for psychologist {PsychologistId}", psyId);
                return Result<IEnumerable<ServiceResponse>>.Success(new List<ServiceResponse>());
            }

            var result = services.Select(_mapper.FromEntity).ToList();

            return Result<IEnumerable<ServiceResponse>>.Success(result);
        }

        public async Task<Result> UpdateAsync(int psyId, UpdateServiceRequest dataRequest, CancellationToken cancellationToken)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == dataRequest.Id,cancellationToken);

            if (service == null)
            {
                _logger.LogWarning("Failed to update service {ServiceId}: service was not found", dataRequest.Id);
                return Result.Failure(404, $"Услуга {dataRequest.Id} не найдена");
            }

            if (service.PsychologistId != psyId)
            {
                _logger.LogWarning("Access denied. Psychologist {PsychologistId} attempted to delete " +
                                    "service {ServiceId} owned by psychologist {OwnerPsychologistId}", psyId, dataRequest.Id, service.PsychologistId);

                return Result.Failure(403, "У вас нет прав для удаления данной услуги");
            }

            int newVersion = service.Version + 1;

            var updateRow = await _context.Services
                                        .Where(s => s.Id == dataRequest.Id &&
                                               s.PsychologistId == psyId &&
                                               s.Version == dataRequest.Version)
                                        .ExecuteUpdateAsync(s =>
                                        {
                                            s.SetProperty(n => n.Name, dataRequest.Name);
                                            s.SetProperty(p => p.Price, dataRequest.Price);
                                            s.SetProperty(v => v.Version, v => v.Version + 1);
                                            s.SetProperty(d => d.UpdateAt, DateTimeOffset.UtcNow);
                                        },cancellationToken);

            if(updateRow == 0)
            {
                _logger.LogWarning("Failed to update service {ServiceId}. " +
                                   "Version conflict. Expected version: {ExpectedVersion}", dataRequest.Id, dataRequest.Version);

                return Result.Failure(409, "Услуга была изменена другим запросом. Обновите данные и попробуйте снова");
            }

            return Result.Success();
        }
    }
}

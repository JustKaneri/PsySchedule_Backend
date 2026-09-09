using Microsoft.EntityFrameworkCore;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;

namespace PsySchedule.Services
{
    public class VacationService : IVacationService
    {
        private readonly DataContext _context;
        private readonly ILogger<VacationService> _logger;
        private readonly IDayService _dayService;

        public VacationService(DataContext context, ILogger<VacationService> logger, IDayService dayService)
        {
            _context = context;
            _logger = logger;
            _dayService = dayService;
        }

        public async Task<Result<IEnumerable<VacationResponse>>> GetAsync(int psyId, CancellationToken cancellationToken)
        {
            var result = await _context.Vacations.Where(v => v.PsychologistId == psyId)
                                               .OrderBy(v => v.StartedAt)
                                               .Select(s => new VacationResponse(s.Id, s.StartedAt, s.FinishedAt))
                                               .ToListAsync(cancellationToken);

            if(result.Count == 0)
            {
                _logger.LogWarning("Vacation not found for {PsychologistId}", psyId);
            }

            return Result<IEnumerable<VacationResponse>>.Success(result);

        }

        public async Task<Result> CancelAsync(int psyId, int vacationId, CancellationToken cancellationToken)
        {
            var vacation = await _context.Vacations.SingleOrDefaultAsync(v => v.Id == vacationId && v.PsychologistId == psyId, cancellationToken);

            if (vacation is null)
            {
                _logger.LogWarning("Vacation {VacationId} not found for psychologist {PsychologistId}", vacationId, psyId);

                return Result.Failure(404, "Отпуск не найден");
            }

            //TODO: Вынести в отдельный сервис
            var scheduleTemplates = await _context.ScheduleTemplates.Where(s => s.PsychologistId == psyId).ToListAsync(cancellationToken);

            _dayService.AddDaysFromTemplate(scheduleTemplates, vacation.StartedAt, vacation.FinishedAt, cancellationToken);

            _context.Vacations.Remove(vacation);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> CreateAsync(int psyId, CreateVacationRequest request, CancellationToken cancellationToken)
        {
            var hasOverlap = await _context.Vacations.AnyAsync(v => v.PsychologistId == psyId &&
                                                          v.StartedAt <= request.FinishedAt &&
                                                          v.FinishedAt >= request.StartedAt, cancellationToken);

            if (hasOverlap)
            {
                _logger.LogWarning("");

                return Result.Failure(409, "Отпуск пересекается с другими отпусками");
            }

            var psyTimeZone = await _context.Psychologists.Where(p => p.Id == psyId).Select(p => p.TimeZone).FirstOrDefaultAsync(cancellationToken);

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(psyTimeZone);

            var localStart = request.StartedAt.ToDateTime(TimeOnly.MinValue);
            var localEnd = request.FinishedAt.AddDays(1).ToDateTime(TimeOnly.MinValue);

            var utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart, timeZone);
            var utcEnd = TimeZoneInfo.ConvertTimeToUtc(localEnd, timeZone);

            var hasAppointment = await _context.Appointments.AnyAsync(a => a.PsychologistId == psyId &&
                                                                           (a.Status == Models.Enums.AppointmentStatus.Confirmed ||
                                                                            a.Status == Models.Enums.AppointmentStatus.Created) &&
                                                                            a.StartedAt >= utcStart &&
                                                                            a.StartedAt < utcEnd,
                                                                            cancellationToken);

            if (hasAppointment)
            {
                _logger.LogWarning("");

                return Result.Failure(409, "Не возможно создать отпуск, при наличие активных записях");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            _context.Vacations.Add(new Vacation()
            {
                StartedAt = request.StartedAt,
                FinishedAt = request.FinishedAt,
                PsychologistId = psyId

            });

            var removeResult = await _dayService.RemoveRangeAsync(psyId, request.StartedAt, request.FinishedAt, cancellationToken);

            if(!removeResult.IsSuccess)
            {
                await _context.Database.RollbackTransactionAsync(cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            await _context.Database.CommitTransactionAsync(cancellationToken);

            return Result.Success();
            
        }
    }
}

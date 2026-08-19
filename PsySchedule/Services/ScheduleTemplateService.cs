using Microsoft.EntityFrameworkCore;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;
using PsySchedule.Models.Enums;

namespace PsySchedule.Services
{
    public class ScheduleTemplateService : IScheduleTemplateService
    {
        private readonly ILogger<ScheduleTemplateService> _logger;
        private readonly DataContext _context;
        private readonly IMapper<ScheduleTemplateDayDto, ScheduleTemplate,int> _mapper;
        private readonly IDayService _dayService;

        public ScheduleTemplateService(ILogger<ScheduleTemplateService> logger, DataContext context, 
                                       IMapper<ScheduleTemplateDayDto, ScheduleTemplate,int> mapper,
                                       IDayService dayService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _dayService = dayService;
        }

        public async Task<Result> CreateAsync(IEnumerable<ScheduleTemplateDayDto> scheduleTemplates, int psychologistId, CancellationToken cancellationToken)
        {
            if(!await _context.Psychologists.AsNoTracking().AnyAsync(p => p.Id == psychologistId))
            {
                _logger.LogWarning("Psychologist {PsychologistId} was not found", psychologistId);
                return Result.Failure(401, "Психолог не найден.");
            }

            if(await _context.ScheduleTemplates.AnyAsync(p => p.PsychologistId == psychologistId))
            {
                _logger.LogWarning("Schedule templates already exist for psychologist {PsychologistId}", psychologistId);
                return Result.Failure(409, "Для психолога уже существуют шаблоны расписания.");
            }


            var templates = scheduleTemplates.Select(st => _mapper.FromDto(st,psychologistId)).ToList();

            var dateFrom = DateOnly.FromDateTime(DateTime.Now);
            var dateTo = dateFrom.AddDays(14);

            _dayService.AddDaysFromTemplate(templates, dateFrom, dateTo, cancellationToken);

            await _context.AddRangeAsync(templates, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<ScheduleTemplatesDto>> GetAsync(int psychologistId, CancellationToken cancellationToken)
        {
            var templates = await _context.ScheduleTemplates
                                          .AsNoTracking()
                                          .Where(st => st.PsychologistId == psychologistId)
                                          .ToListAsync(cancellationToken);

            if (templates.Count() == 0)
            {
                _logger.LogWarning("Schedule templates not exist for psychologist {PsychologistId}", psychologistId);
                return Result<ScheduleTemplatesDto>.Failure(404, "Расписание не найдено");
            }

            List<ScheduleTemplateDayDto> scheduleTemplates = new List<ScheduleTemplateDayDto>();

            var days = templates.Select(_mapper.FromEntity).ToList();

            return Result<ScheduleTemplatesDto>.Success(new ScheduleTemplatesDto(scheduleTemplates));
        }

        public async Task<Result> UpdateOrCreateAsync(ScheduleTemplateDayDto scheduleTemplate, int psychologistId, CancellationToken cancellationToken)
        {
            if (!await _context.Psychologists.AsNoTracking().AnyAsync(p => p.Id == psychologistId, cancellationToken))
            {
                _logger.LogWarning("Psychologist {PsychologistId} was not found", psychologistId);
                return Result.Failure(401, "Психолог не найден.");
            }


            var template = await _context.ScheduleTemplates.FirstOrDefaultAsync(st => st.PsychologistId == psychologistId && st.Weekend == (WeekDay)scheduleTemplate.Weekday,cancellationToken);

            if(template == null)
            {
                template = _mapper.FromDto(scheduleTemplate, psychologistId);

                await _context.AddAsync(template);

                var dateFrom = DateOnly.FromDateTime(DateTime.Now);
                var dateTo = dateFrom.AddDays(14);

                _dayService.AddDaysFromTemplate(new[] {template}, dateFrom, dateTo, cancellationToken);
            }
            else
            {
                template.Gap = scheduleTemplate.Gap;
                template.StartedAt = TimeOnly.Parse(scheduleTemplate.WorkTime.StartedAt);
                template.FinishedAt = TimeOnly.Parse(scheduleTemplate.WorkTime.FinishedAt);
                template.BreakStartedAt = TimeOnly.Parse(scheduleTemplate.BreakTime.StartedAt);
                template.BreakFinishedAt = TimeOnly.Parse(scheduleTemplate.BreakTime.FinishedAt);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

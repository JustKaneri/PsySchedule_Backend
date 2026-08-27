using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;
using PsySchedule.Models.Enums;

namespace PsySchedule.Services
{
    public class DayService : IDayService
    {
        private readonly DataContext _context;
        private readonly ILogger<DayService> _logger;

        public DayService(DataContext context, ILogger<DayService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddDaysFromTemplate(IEnumerable<ScheduleTemplate> templates, DateOnly from, DateOnly to, CancellationToken cancellationToken)
        {
            var templatesByDay = templates.ToDictionary(template => template.Weekday);

            List<WorkDay> workDays = new();

            for(DateOnly date = from; date < to; date = date.AddDays(1))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var weekDay = GetWeekDay(date);

                if (!templatesByDay.TryGetValue(weekDay, out var template))
                    continue;

                workDays.Add(new WorkDay()
                {
                    Date = date,
                    StartedAt = template.StartedAt,
                    FinishedAt = template.FinishedAt,
                    BreakStartedAt = template.BreakStartedAt,
                    BreakFinishedAt = template.BreakFinishedAt,
                    Gap = template.Gap,
                    PsychologistId = template.PsychologistId,
                    ScheduleTemplate = template
                });
            }

            _context.AddRange(workDays);
        }

        public Task<Result> GenerateNextDayAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateDayAsync(ScheduleDayDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private static WeekDay GetWeekDay(DateOnly date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Monday => WeekDay.Monday,
                DayOfWeek.Tuesday => WeekDay.Tuesday,
                DayOfWeek.Wednesday => WeekDay.Wednesday,
                DayOfWeek.Thursday => WeekDay.Thursday,
                DayOfWeek.Friday => WeekDay.Friday,
                DayOfWeek.Saturday => WeekDay.Saturday,
                DayOfWeek.Sunday => WeekDay.Sunday,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

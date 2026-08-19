using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;
using PsySchedule.Models.Enums;

namespace PsySchedule.Mapper
{
    public class ScheduleTemplateMapper : IMapper<ScheduleTemplateDayDto, ScheduleTemplate, int>
    {
        public ScheduleTemplate FromDto(ScheduleTemplateDayDto dto, int psychologistId)
        {
            return new ScheduleTemplate()
            {
                Weekend = (WeekDay)dto.Weekday,
                StartedAt = TimeOnly.Parse(dto.WorkTime.StartedAt),
                FinishedAt = TimeOnly.Parse(dto.WorkTime.FinishedAt),
                BreakStartedAt = TimeOnly.Parse(dto.BreakTime.StartedAt),
                BreakFinishedAt = TimeOnly.Parse(dto.BreakTime.FinishedAt),
                Gap = dto.Gap,
                PsychologistId = psychologistId
            };
        }

        public ScheduleTemplateDayDto FromEntity(ScheduleTemplate entity)
        {
            return new((int)entity.Weekend,
                      new TimeRange(
                          entity.StartedAt.ToString("HH:mm"),
                          entity.FinishedAt.ToString("HH:mm")),
                      new TimeRange(
                          entity.BreakStartedAt.ToString("HH:mm"),
                          entity.BreakFinishedAt.ToString("HH:mm")),
                      entity.Gap);
        }
    }
}

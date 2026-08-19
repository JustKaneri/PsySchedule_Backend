using PsySchedule.Models;

namespace PsySchedule.Dto
{
    /// <summary>
    /// Шаблон дня расписания
    /// </summary>
    /// <param name="Weekday">День недели</param>
    /// <param name="WorkTime">Рабочие часы</param>
    /// <param name="BreakTime">Время обеда</param>
    /// <param name="Gap">Перерыв между сессиями</param>
    public record ScheduleTemplateDayDto(int Weekday, TimeRange WorkTime, TimeRange BreakTime, int Gap);

    /// <summary>
    /// Шаблон для обновления дня расписания
    /// </summary>
    /// <param name="Weekday">День недели</param>
    /// <param name="WorkTime">Рабочие часы</param>
    /// <param name="BreakTime">Время обеда</param>
    /// <param name="Gap">Перерыв между сессиями</param>
    public record UpdateScheduleTemplateDayDto(bool IsUpdateDays, int Weekday, TimeRange WorkTime, TimeRange BreakTime, int Gap) : ScheduleTemplateDayDto(Weekday, WorkTime, BreakTime, Gap);

    /// <summary>
    /// Шаблон расписания
    /// </summary>
    /// <param name="TemplateDays">шаблон дней</param>
    public record ScheduleTemplatesDto(IReadOnlyCollection<ScheduleTemplateDayDto> TemplateDays);

    /// <summary>
    /// Временой отрезок
    /// </summary>
    /// <param name="StartedAt">Начало</param>
    /// <param name="FinishedAt">Конец</param>
    public record TimeRange(string StartedAt, string FinishedAt);
}

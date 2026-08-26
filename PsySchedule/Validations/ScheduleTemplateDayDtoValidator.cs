using FluentValidation;
using PsySchedule.Dto;

namespace PsySchedule.Validations
{
    public class ScheduleTemplateDayDtoValidator : AbstractValidator<ScheduleTemplateDayDto>
    {
        public ScheduleTemplateDayDtoValidator()
        {
            RuleFor(p => p.Weekday)
                 .InclusiveBetween(1, 7)
                 .WithMessage("Weekday должен иметь значени от 1 до 7.");

            RuleFor(p => p.WorkTime)
                 .NotNull()
                 .SetValidator(new TimeRangeValidator());

            RuleFor(p => p.BreakTime)
                 .NotNull()
                 .SetValidator(new TimeRangeValidator());

            RuleFor(p => p)
                .Must(x =>
                {
                    if (!TimeOnly.TryParse(x.WorkTime.StartedAt, out var startedAt) ||
                       !TimeOnly.TryParse(x.WorkTime.FinishedAt, out var finishedAt) ||
                       !TimeOnly.TryParse(x.BreakTime.StartedAt, out var breakStart) ||
                       !TimeOnly.TryParse(x.BreakTime.FinishedAt, out var breakEnd))
                            return false;

                    // Перерыв должен быть внутри рабочего времени
                    if (breakStart < startedAt || breakEnd > finishedAt)
                        return false;

                    // Окончание перерыва не может быть раньше его начала
                    if (breakEnd < breakStart)
                        return false;


                    return true;
                })
                .WithMessage("Переыв должен быть в рамках рабочего времени");

            RuleFor(p => p.Gap)
                 .InclusiveBetween(0, 60)
                 .WithMessage("Gap должен иметь значени от 0 до 60.");
        }
    }
}

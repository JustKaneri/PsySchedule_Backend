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

            RuleFor(p => p.Gap)
                 .InclusiveBetween(0, 60)
                 .WithMessage("Gap должен иметь значени от 0 до 60.");
        }
    }
}

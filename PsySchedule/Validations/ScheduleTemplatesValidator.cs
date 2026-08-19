using FluentValidation;
using PsySchedule.Dto;

namespace PsySchedule.Validations
{
    public class ScheduleTemplatesValidator : AbstractValidator<IEnumerable<ScheduleTemplateDayDto>>
    {
        public ScheduleTemplatesValidator()
        {
            RuleFor(p => p)
                .NotEmpty();

            RuleForEach(p => p)
                .NotNull()
                .SetValidator(new ScheduleTemplateDayDtoValidator());

            RuleFor(p => p)
                .Must(p => p.Select(s => s.Weekday).Distinct().Count() == p.Count())
                .WithMessage("Каждый день недели должен быть уникальным.");
        }
    }
}

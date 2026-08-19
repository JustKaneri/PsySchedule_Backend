using FluentValidation;
using PsySchedule.Dto;

namespace PsySchedule.Validations
{
    public class TimeRangeValidator : AbstractValidator<TimeRange>
    {
        public TimeRangeValidator()
        {
            RuleFor(p => p.StartedAt)
                .NotEmpty()
                .Matches(@"^([01]\d|2[0-3]):[0-5]\d$")
                .WithMessage("StartedAt должен быть в HH:mm формате.");

            RuleFor(p => p.FinishedAt)
                .NotEmpty()
                .Matches(@"^([01]\d|2[0-3]):[0-5]\d$")
                .WithMessage("FinishedAt должен быть в HH:mm формате.");

            RuleFor(p => p)
                .Must(p =>
                {
                    var start = TimeOnly.Parse(p.StartedAt);
                    var end = TimeOnly.Parse(p.FinishedAt);

                    return start < end;

                }).WithMessage("StartedAt должен быть раньше чем FinishedAt.");
        }
    }
}

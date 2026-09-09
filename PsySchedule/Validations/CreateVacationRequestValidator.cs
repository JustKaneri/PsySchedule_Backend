using FluentValidation;
using PsySchedule.Dto;

namespace PsySchedule.Validations
{
    public class CreateVacationRequestValidator : AbstractValidator<CreateVacationRequest>
    {
        public CreateVacationRequestValidator()
        {
            RuleFor(x => x.StartedAt)
                .NotEmpty()
                .WithMessage("Дата начала отпуска обязательна"); 

            RuleFor(x => x.FinishedAt)
                .NotEmpty()
                .WithMessage("Дата окончания отпуска обязательна");
            
            RuleFor(x => x)
                .Must(x => x.StartedAt <= x.FinishedAt)
                .WithMessage("Дата начала отпуска не может быть позже даты окончания");
        }
    }
}

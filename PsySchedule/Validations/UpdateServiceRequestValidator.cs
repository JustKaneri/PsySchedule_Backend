using FluentValidation;
using PsySchedule.Dto;

namespace PsySchedule.Validations
{
    public class UpdateServiceRequestValidator : AbstractValidator<UpdateServiceRequest>
    {
        public UpdateServiceRequestValidator()
        {
            Include(new ServiceDtoValidator());

            RuleFor(s => s.Id).GreaterThan(0).WithMessage("Id должен быть больше 0");

            RuleFor(s => s.Version).GreaterThan(0).WithMessage("Версия должна быть больше 0");
        }
    }
}

using FluentValidation;
using PsySchedule.Dto;

namespace PsySchedule.Validations
{
    public class ServiceDtoValidator : AbstractValidator<ServiceDto>
    {
        public ServiceDtoValidator()
        {
            RuleFor(s => s.Name)
                    .NotEmpty()
                    .MaximumLength(100)
                    .WithMessage("Название услуги не должно быть пустым и должно быть короче 100 символов");

            RuleFor(s => s.Price)
                   .GreaterThan(0)
                   .LessThan(500_000)
                   .WithMessage("Цена должна быть в диапазоне от 0 до 500000");
        }
    }
}

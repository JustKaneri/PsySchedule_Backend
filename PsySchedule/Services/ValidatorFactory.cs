using FluentValidation;
using FluentValidation.Results;
using PsySchedule.Interfaces;

namespace PsySchedule.Services
{
    public class ValidatorFactory : Interfaces.IValidatorFactory
    {
        private readonly IServiceProvider _services;

        public ValidatorFactory(IServiceProvider services)
        {
            _services = services;
        }

        public ValidationResult Validate<T>(T value)
        {
            var validator = _services.GetRequiredService<IValidator<T>>();

            return validator.Validate(value);
        }
    }
}

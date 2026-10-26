using FluentValidation.Results;

namespace PsySchedule.Interfaces
{
    public interface IValidatorFactory
    {
        public ValidationResult Validate<T>(T value);
    }
}

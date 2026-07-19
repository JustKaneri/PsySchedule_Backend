using FluentValidation;
using PsySchedule.Dto;

namespace PsySchedule.Validations
{
    /// <summary>
    /// Валидация DTO для регистрации психолога
    /// </summary>
    public class PsychologistRegistrationValidator : AbstractValidator<RegisterPsychologistDto>
    {
        public PsychologistRegistrationValidator()
        {
            RuleFor(n => n.Name)
                    .NotEmpty()
                        .WithMessage("Имя не может быть пустым")
                    .MinimumLength(6)
                        .WithMessage("Длина имени должна быть от 6 символов")
                    .MaximumLength(50)
                        .WithMessage("Длина логина должна быть до 5 символов");


            RuleFor(l => l.Login)
                    .NotEmpty()
                        .WithMessage("Логин не может быть пустым")
                    .MinimumLength(6)
                        .WithMessage("Длина логина должна быть от 6 символов")
                    .MaximumLength(30)
                        .WithMessage("Длина логина должна быть до 5 символов");

            RuleFor(p => p.Password)
                    .NotEmpty()
                        .WithMessage("Пароль не может быть пустым")
                    .MinimumLength(8)
                        .WithMessage("Пароль должен быть от 8 символов")
                    .MaximumLength(32)
                        .WithMessage("Длина пароля должна быть до 32 символа")
                    .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,32}$")
                        .WithMessage("пароль должен содержать хотя бы одну цифра, одну строчную и одну заглавнаю буква");


            RuleFor(t => t.TimeZone)
                    .NotEmpty()
                        .WithMessage("Часовой пояс не может быть пустым")
                    .Must(BeValidTimeZone)
                        .WithMessage("Некорректный часовой пояс.");
        }

        private bool BeValidTimeZone(string timeZoneId)
        {
            return TimeZoneInfo.GetSystemTimeZones()
                               .Any(tz => tz.Id == timeZoneId);
        }
    }
}

using FluentValidation;
using PsySchedule.Dto;

namespace PsySchedule.Validations
{
    public class AuthenticationValidator : AbstractValidator<AuthenticationDto>
    {
        public AuthenticationValidator()
        {
            RuleFor(l => l.Login)
                    .NotEmpty()
                        .WithMessage("Логин не может быть пустым")
                    .MinimumLength(6)
                        .WithMessage("Длина логина должна быть от 6 символов")
                    .MaximumLength(30)
                        .WithMessage("Длина логина должна быть до 30 символов");

            RuleFor(p => p.Password)
                    .NotEmpty()
                        .WithMessage("Пароль не может быть пустым")
                    .MinimumLength(8)
                        .WithMessage("Пароль должен быть от 8 символов")
                    .MaximumLength(32)
                        .WithMessage("Длина пароля должна быть до 32 символов")
                    .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,32}$")
                        .WithMessage("Пароль должен содержать хотя бы одну цифру, одну строчную и одну заглавную букву");
        }
    }
}

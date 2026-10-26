using FluentValidation;
using GrpcClient = PsySchedule.GRPC;

namespace PsySchedule.Validations
{
    public class TelegramDataValidator : AbstractValidator<GrpcClient.TelegramData>
    {
        public TelegramDataValidator()
        {
            RuleFor(x => x.Id)
               .GreaterThan(0)
               .WithMessage("Идентификатор пользователя Telegram должен быть больше нуля.");

            RuleFor(x => x.TelegramChatId)
                .NotEqual(0L)
                .WithMessage("Идентификатор чата Telegram не должен быть равен нулю.");

            RuleFor(x => x.UserName)
                .MaximumLength(32)
                .WithMessage("Имя пользователя Telegram не должно превышать 32 символа.")
                .Matches(@"^[a-zA-Z0-9_]+$")
                .WithMessage("Имя пользователя Telegram может содержать только латинские буквы, цифры и символ подчёркивания.")
                .When(x => !string.IsNullOrWhiteSpace(x.UserName));

            RuleFor(x => x.FirstName)
                .MaximumLength(100)
                .WithMessage("Имя в Telegram не должно превышать 100 символов.")
                .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(100)
                .WithMessage("Фамилия в Telegram не должна превышать 100 символов.")
                .When(x => !string.IsNullOrWhiteSpace(x.LastName));

            RuleFor(x => x.LanguageCode)
                .MaximumLength(16)
                .WithMessage("Код языка Telegram не должен превышать 16 символов.")
                .When(x => !string.IsNullOrWhiteSpace(x.LanguageCode));
        }
    }
}

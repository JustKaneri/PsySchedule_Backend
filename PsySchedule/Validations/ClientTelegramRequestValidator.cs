using FluentValidation;
using GrpcClient = PsySchedule.GRPC;


namespace PsySchedule.Validations
{
    public class ClientTelegramRequestValidator : AbstractValidator<GrpcClient.ClientTelegramRequest>
    {
        public ClientTelegramRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Имя обязательно для заполнения.")
                .MaximumLength(100)
                .WithMessage("Имя не должно превышать 100 символов.");

            RuleFor(x => x.SecondName)
                .MaximumLength(100)
                .WithMessage("Фамилия не должна превышать 100 символов.")
                .When(x => !string.IsNullOrWhiteSpace(x.SecondName));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{7,14}$")
                .WithMessage("Номер телефона должен быть указан в международном формате, например +79991234567.")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone));

            RuleFor(x => x.TimeZone)
                .NotEmpty()
                .WithMessage("Часовой пояс обязателен для заполнения.")
                .Must(BeValidTimeZone)
                .WithMessage("Указан некорректный часовой пояс.");

            RuleFor(x => x.TelegramData)
                .NotNull()
                .WithMessage("Данные Telegram обязательны для заполнения.")
                .SetValidator(new TelegramDataValidator()!);
        }

        private static bool BeValidTimeZone(string? timeZone)
        {
            if (string.IsNullOrWhiteSpace(timeZone))
                return false;

            try
            {
                TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                return true;
            }
            catch (TimeZoneNotFoundException)
            {
                return false;
            }
            catch (InvalidTimeZoneException)
            {
                return false;
            }
        }
    }
}

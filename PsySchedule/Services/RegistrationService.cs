using Microsoft.EntityFrameworkCore;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Health;
using PsySchedule.Interfaces;
using PsySchedule.Models;
using System.Security.Cryptography;

namespace PsySchedule.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ILogger<RegistrationService> _logger;

        public RegistrationService(DataContext context, IPasswordHasher passwordHasher,
                                   ITokenService tokenService,
                                   ILogger<RegistrationService> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<AuthTokensDto>> RegisterPsychologistAsync(RegisterPsychologistDto registerDto, MetaDataDto userData, CancellationToken cancellationToken)
        {
            string normalizationLogin = registerDto.Login.Trim().ToLowerInvariant();

            var exists = await _context.Psychologists
                                        .AnyAsync(ps => ps.Login == normalizationLogin, cancellationToken);

            if (exists)
                return Result<AuthTokensDto>.Failure(400, "Не корекктные данные для регистрации");

            string salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

            Psychologist psychologist = new Psychologist()
            {
                Login = normalizationLogin,
                Name = registerDto.Name.Trim(),
                Salt = salt,
                Password = _passwordHasher.Hash(registerDto.Password, salt),
                TimeZone = registerDto.TimeZone
            };

            Token token = null;

            await using (var tran = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await _context.Psychologists.AddAsync(psychologist,cancellationToken);

                    await _context.SaveChangesAsync(cancellationToken);

                    token = _tokenService.CreateToken(psychologist.Id, userData);

                    token.Psychologist = psychologist;

                    await _context.AddAsync(token,cancellationToken);

                    await _context.SaveChangesAsync(cancellationToken);

                    await tran.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, "Ошибка регистрации пользователя {Login}", registerDto.Login);
                    await tran.RollbackAsync(cancellationToken);
                    return Result<AuthTokensDto>.Failure(400, "Не удалось создать пользователя");
                }
            }

            HealthService.RegistrationsCountMetric.Inc();

            return Result<AuthTokensDto>.Success(new AuthTokensDto(token.TokenAccess, token.TokenRefresh));
        }
    }
}

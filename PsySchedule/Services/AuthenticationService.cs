using Microsoft.EntityFrameworkCore;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Health;
using PsySchedule.Interfaces;
using PsySchedule.Models;

namespace PsySchedule.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordService;
        private readonly DataContext _context;

        public AuthenticationService(ILogger<AuthenticationService> logger, ITokenService tokenService,
                                     IPasswordHasher passwordService, DataContext context)
        {
            _logger = logger;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _context = context;
        }

        public async Task<Result<AuthTokensDto>> AuthenticateAsync(AuthenticationDto authenticationData, MetaDataDto metaData, CancellationToken cancellationToken)
        {
            string normalLogin = authenticationData.Login.Trim().ToLowerInvariant();

            var user = await _context.Psychologists.AsNoTracking()
                                                   .Select(s => new { s.Id, s.Login, s.Password, s.Salt })
                                                   .FirstOrDefaultAsync(ps => ps.Login == normalLogin, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning($"Failed attempt аuthenticate login {normalLogin}");
                return Result<AuthTokensDto>.Failure(401, "Логин или пароль не совпадает");
            }

            if (!_passwordService.Verify(authenticationData.Password, user.Password, user.Salt))
            {
                _logger.LogWarning($"Failed attempt аuthenticate login {normalLogin} with not correct password");
                return Result<AuthTokensDto>.Failure(401, "Логин или пароль не совпадает");
            }

            var token = _tokenService.CreateToken(user.Id, metaData);

            await _context.Tokens.AddAsync(token, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            HealthService.AuthenticationCountMetric.Inc();

            return Result<AuthTokensDto>.Success(new(token.TokenAccess, token.TokenRefresh));
        }

        public async Task<Result<bool>> LogoutAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var countRow = await _context.Tokens
                                         .Where(tk => tk.TokenRefresh == refreshToken &&
                                                !tk.IsUsed &&
                                                !tk.IsRevoked)
                                         .ExecuteUpdateAsync(tk => tk.SetProperty(p => p.IsRevoked, true), cancellationToken);

            if (countRow == 0)
            {
                return Result<bool>.Failure(401, "Токен не действителен");
            }


            return Result<bool>.Success(true);
        }

        public async Task<Result<AuthTokensDto>> RefreshTokenAsync(string refreshToken, MetaDataDto metaData, CancellationToken cancellationToken)
        {
            var refToken = await _context.Tokens.FirstOrDefaultAsync(tk => tk.TokenRefresh == refreshToken &&
                                                                   !tk.IsUsed &&
                                                                   !tk.IsRevoked, cancellationToken);

            if (refToken == null)
            {
                return Result<AuthTokensDto>.Failure(401, "Refresh токен не найден");
            }

            if (refToken.ExpiresAt <= DateTime.UtcNow)
            {
                return Result<AuthTokensDto>.Failure(401, "Refresh токен истек");
            }

            if (refToken.Ip != metaData.Ip)
            {
                _logger.LogInformation("Refresh token used from another IP. Old={Old}, New={New}", refToken.Ip, metaData.Ip);
            }

            if (!string.Equals(refToken.UserAgent, metaData.UserAgent, StringComparison.Ordinal))
            {
                _logger.LogInformation("Refresh token used from another User-Agent. Old={Old}, New={New}", refToken.UserAgent, metaData.UserAgent);
            }

            Token token = new Token();

            await using var tran = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var countRow = await _context.Tokens
                                     .Where(tk => tk.TokenRefresh == refreshToken &&
                                            !tk.IsUsed &&
                                            !tk.IsRevoked &&
                                             tk.ExpiresAt > DateTime.UtcNow)
                                     .ExecuteUpdateAsync(tk => tk.SetProperty(p => p.IsUsed, true), cancellationToken);

                if (countRow == 0)
                {
                    _logger.LogWarning("Attempt to reuse refresh token: {RefreshToken}", refreshToken);
                    return Result<AuthTokensDto>.Failure(409, "Refresh токен был использован");
                }

                token = _tokenService.CreateToken(refToken.PsychologistId, metaData);

                await _context.Tokens.AddAsync(token, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                await tran.CommitAsync(cancellationToken);

                return Result<AuthTokensDto>.Success(new AuthTokensDto(token.TokenAccess, token.TokenRefresh));

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed refresh token");

                return Result<AuthTokensDto>.Failure(400, "Во время обновления токена, возникла ошибка");
            }

        }
    }
}

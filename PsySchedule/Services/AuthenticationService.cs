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

        public async Task<Result<AuthTokensDto>> AuthenticateAsync(AuthenticationDto authenticationData, UserDataDto userData, CancellationToken cancellationToken)
        {
            string normalLogin = authenticationData.Login.Trim().ToLowerInvariant();

            var user = await _context.Psychologists.AsNoTracking()
                                                   .Select(s => new {s.Id, s.Login, s.Password, s.Salt})
                                                   .FirstOrDefaultAsync(ps => ps.Login == normalLogin, cancellationToken);

            if (user == null )
            {
                _logger.LogWarning($"Failed attempt аuthenticate login {normalLogin}");
                return Result<AuthTokensDto>.Failure(401, "Логин или пароль не совпадает");
            }

            if(!_passwordService.Verify(authenticationData.Password, user.Password, user.Salt))
            {
                _logger.LogWarning($"Failed attempt аuthenticate login {normalLogin} with not correct password");
                return Result<AuthTokensDto>.Failure(401, "Логин или пароль не совпадает");
            }

            var token = _tokenService.CreateToken(user.Id, userData);

            await _context.Tokens.AddAsync(token,cancellationToken);

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
                                         .ExecuteUpdateAsync(tk => tk.SetProperty(p => p.IsRevoked, true),cancellationToken);

            if (countRow == 0)
            {
                return Result<bool>.Failure(401, "Токен не действителен");
            }

            
            return Result<bool>.Success(true);
        }

        public Task<Result<AuthTokensDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

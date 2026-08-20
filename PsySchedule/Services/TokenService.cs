using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PsySchedule.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenParameters _parameters;

        public TokenService(IOptions<TokenParameters> parameters)
        {
            _parameters = parameters.Value;
        }

        public Token CreateToken(int userId, MetaDataDto userData)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //TODO кешировать в будущем
            var secretKey = Encoding.UTF8.GetBytes(_parameters.Key);
            var timeLive = TimeSpan.Parse(_parameters.ExpireTime);
            var timeLiveRefresh = int.Parse(_parameters.ExpireTimeRefresh);

            var subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64),
            });

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = subject,
                Expires = DateTime.UtcNow.Add(timeLive),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtString = tokenHandler.WriteToken(jwtToken);

            var token = new Token()
            {
                PsychologistId = userId,
                TokenRefresh = GenerateRefresh(),
                TokenAccess = jwtString,
                UserAgent = userData.UserAgent,
                Ip = userData.Ip,
                IsRevoked = false,
                IsUsed = false,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMonths(timeLiveRefresh)
            };

            return token;
        }

        private string GenerateRefresh()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public bool VerificationToken()
        {
            throw new NotImplementedException();
        }
    }
}

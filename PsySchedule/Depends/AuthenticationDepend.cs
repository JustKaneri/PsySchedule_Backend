using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PsySchedule.Context;
using PsySchedule.Models;
using System.Text;

namespace PsySchedule.Depends
{
    public static class AuthenticationDepend
    {
        public static WebApplicationBuilder UseAuthentication(this WebApplicationBuilder builder)
        {
            var jwt = builder.Configuration.GetSection("Jwt").Get<TokenParameters>()!;

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                ValidIssuer = jwt.ValidIssuer,
                ValidateIssuer = jwt.ValidateIssuer,
                ValidateAudience = jwt.ValidateAudience,
                ValidateLifetime = jwt.ValidateLifetime,
                ClockSkew = TimeSpan.Zero
            };


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(jwt =>
                            {
                                 jwt.SaveToken = true;
                                 jwt.TokenValidationParameters = tokenValidationParameters;
                            });

            return builder;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace PsyScheduleTests.ServiceTests
{
    public class AuthenticationTests : BaseTestClass, IClassFixture<PostgreSqlFixture>
    {
        private readonly ITestOutputHelper _outputHelper;

        public AuthenticationTests(PostgreSqlFixture fixture, ITestOutputHelper outputHelper) : base(fixture)
        {
            _outputHelper = outputHelper;
        }

        /// <summary>
        /// Аутентификация с корректными данными 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Auth_Correct_Test()
        {
            using var scope = CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IRegistrationService>();

            var regData = new RegisterPsychologistDto("Иван", "Иван123", "1234", "Russian Standard Time");
            var usData = new UserDataDto("192.168.0.1", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

            await service.RegisterPsychologistAsync(regData, usData, CancellationToken.None);

            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var authData = new AuthenticationDto("Иван123", "1234");

            var resutl = await serviceAuth.AuthenticateAsync(authData,usData, CancellationToken.None);

            Assert.True(resutl.IsSuccess);
            Assert.NotNull(resutl.Value);
        }


        /// <summary>
        /// Аутентификация не сущ. пользователя
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Auth_Not_Exist_Test()
        {
            var usData = new UserDataDto("192.168.0.1", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var authData = new AuthenticationDto("Иван123", "1234");

            var resutl = await serviceAuth.AuthenticateAsync(authData, usData, CancellationToken.None);

            Assert.False(resutl.IsSuccess);
            Assert.Equal(401, resutl.Error.errorCode);
        }

        /// <summary>
        /// Аутентификация с не правильным паролем
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Auth_Not_Correct_Password_Test()
        {
            using var scope = CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IRegistrationService>();

            var regData = new RegisterPsychologistDto("Иван", "Иван123", "1234", "Russian Standard Time");
            var usData = new UserDataDto("192.168.0.1", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

            await service.RegisterPsychologistAsync(regData, usData, CancellationToken.None);

            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var authData = new AuthenticationDto("Иван123", "12345");

            var resutl = await serviceAuth.AuthenticateAsync(authData, usData, CancellationToken.None);

            Assert.False(resutl.IsSuccess);
            Assert.Equal(401, resutl.Error.errorCode);
        }

        /// <summary>
        /// Параллельная аутентификация
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Auth_Parallel_Test()
        {
            using var scope = CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IRegistrationService>();

            var regData = new RegisterPsychologistDto("Иван", "Иван123", "1234", "Russian Standard Time");
            var usData = new UserDataDto("192.168.0.1", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

            await service.RegisterPsychologistAsync(regData, usData, CancellationToken.None);

            using var scopeAuth1 = CreateScope();
            using var scopeAuth2 = CreateScope();
            using var scopeVerification = CreateScope();

            var serviceAuth1 = scopeAuth1.ServiceProvider.GetRequiredService<IAuthenticationService>();
            var serviceAuth2 = scopeAuth2.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var authData = new AuthenticationDto("Иван123", "1234");

            var resutl1 = serviceAuth1.AuthenticateAsync(authData, usData, CancellationToken.None);
            var resutl2 = serviceAuth2.AuthenticateAsync(authData, usData, CancellationToken.None);

            Task.WaitAll(resutl1, resutl2);

            Assert.True(resutl1.Result.IsSuccess);
            Assert.True(resutl2.Result.IsSuccess);

            Assert.NotEqual(resutl1.Result.Value.AccessToken, resutl2.Result.Value.AccessToken);
            Assert.NotEqual(resutl1.Result.Value.RefreshToken, resutl2.Result.Value.RefreshToken);

            var context = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            //1-а Запись - токены при регистрации
            //2-е Записи - авторизация
            Assert.Equal(3, context.Tokens.Count());
        }
    }
}

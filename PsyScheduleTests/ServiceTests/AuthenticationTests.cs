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
            var reg = await Registration();

            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var authData = new AuthenticationDto("Иван123", "1234");

            var result = await serviceAuth.AuthenticateAsync(authData,usData, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }


        /// <summary>
        /// Аутентификация не сущ. пользователя
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Auth_Not_Exist_Test()
        {
            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var authData = new AuthenticationDto("Иван123", "1234");

            var result = await serviceAuth.AuthenticateAsync(authData, usData, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(401, result.Error.ErrorCode);
        }

        /// <summary>
        /// Аутентификация с не правильным паролем
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Auth_Not_Correct_Password_Test()
        {
            await Registration();

            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var authData = new AuthenticationDto("Иван123", "12345");

            var result = await serviceAuth.AuthenticateAsync(authData, usData, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(401, result.Error.ErrorCode);
        }

        /// <summary>
        /// Параллельная аутентификация
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Auth_Parallel_Test()
        {
            await Registration();

            using var scopeAuth1 = CreateScope();
            using var scopeAuth2 = CreateScope();
            using var scopeVerification = CreateScope();

            var serviceAuth1 = scopeAuth1.ServiceProvider.GetRequiredService<IAuthenticationService>();
            var serviceAuth2 = scopeAuth2.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var authData = new AuthenticationDto("Иван123", "1234");

            var result1 = serviceAuth1.AuthenticateAsync(authData, usData, CancellationToken.None);
            var result2 = serviceAuth2.AuthenticateAsync(authData, usData, CancellationToken.None);

            Task.WaitAll(result1, result2);

            Assert.True(result1.Result.IsSuccess);
            Assert.True(result2.Result.IsSuccess);

            Assert.NotEqual(result1.Result.Value.AccessToken, result2.Result.Value.AccessToken);
            Assert.NotEqual(result1.Result.Value.RefreshToken, result2.Result.Value.RefreshToken);

            var context = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            //1-а Запись - токены при регистрации
            //2-е Записи - авторизация
            Assert.Equal(3, context.Tokens.Count());
        }

        /// <summary>
        /// Корректное завершение сессии
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Logout_Correct_Test()
        {
            var regRes = await Registration();

            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var result = await serviceAuth.LogoutAsync(regRes.RefreshToken, CancellationToken.None);

            using var scopeVerification = CreateScope();
            var context = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            Assert.True(result.IsSuccess);
            Assert.Equal(1, context.Tokens.Count());
            Assert.True(context.Tokens.First().IsRevoked);
        }

        /// <summary>
        /// Завершение сессии два раза подряд
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Logout_Twice_Test()
        {
            var regRes = await Registration();

            using var scopeAuth = CreateScope();
            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var result = await serviceAuth.LogoutAsync(regRes.RefreshToken, CancellationToken.None);
            var result2 = await serviceAuth.LogoutAsync(regRes.RefreshToken, CancellationToken.None);

            using var scopeVerification = CreateScope();
            var context = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            Assert.True(result.IsSuccess);
            Assert.False(result2.IsSuccess);
            Assert.Equal(1, context.Tokens.Count());
            Assert.True(context.Tokens.First().IsRevoked);
        }

        /// <summary>
        /// Завершение не сущ. сессии
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Logout_Not_Exist_Token_Test()
        {
            using var scopeAuth = CreateScope();
            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var result = await serviceAuth.LogoutAsync("", CancellationToken.None);

            using var scopeVerification = CreateScope();
            var context = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            Assert.False(result.IsSuccess);
            Assert.Equal(401, result.Error.ErrorCode);
        }

        /// <summary>
        /// Корректное обновление токенов
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Refresh_Correct_Test()
        {
            var regRes = await Registration();

            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var result = await serviceAuth.RefreshTokenAsync(regRes.RefreshToken, usData, CancellationToken.None);

            using var scopeVerification = CreateScope();
            var context = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            Assert.True(result.IsSuccess);
            Assert.Equal(2, context.Tokens.Count());
            Assert.True(context.Tokens.First().IsUsed);
        }

        /// <summary>
        /// Обновление использованого токена
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Refresh_Used_Token_Test()
        {
            var regRes = await Registration();

            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var result = await serviceAuth.RefreshTokenAsync(regRes.RefreshToken, usData, CancellationToken.None);
            var result2 = await serviceAuth.RefreshTokenAsync(regRes.RefreshToken, usData, CancellationToken.None);

            using var scopeVerification = CreateScope();
            var context = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            Assert.True(result.IsSuccess);
            Assert.False(result2.IsSuccess);
            Assert.Equal(2, context.Tokens.Count());
            Assert.True(context.Tokens.First().IsUsed);
        }

        /// <summary>
        /// Race-condition обновления токенов
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Refresh_Parallel_Test()
        {
            var regRes = await Registration();

            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            using var scope1 = CreateScope();
            var service1 = scope1.ServiceProvider.GetRequiredService<IAuthenticationService>();

            using var scope2 = CreateScope();
            var service2 = scope2.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var result = service1.RefreshTokenAsync(regRes.RefreshToken, usData, CancellationToken.None);
            var result2 = service2.RefreshTokenAsync(regRes.RefreshToken, usData, CancellationToken.None);

            Task.WaitAll(result, result2);

            using var scopeVerification = CreateScope();
            var context = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            Assert.True(result.Result.IsSuccess ^ result2.Result.IsSuccess);
            Assert.Equal(2, context.Tokens.Count());
            Assert.True(context.Tokens.First().IsUsed);
        }

        /// <summary>
        /// Обновление не сущ. токена
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Refresh_Not_Exist_Test()
        {
            using var scopeAuth = CreateScope();

            var serviceAuth = scopeAuth.ServiceProvider.GetRequiredService<IAuthenticationService>();

            var result = await serviceAuth.RefreshTokenAsync("123", usData, CancellationToken.None);

            using var scopeVerification = CreateScope();
            var context = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            Assert.False(result.IsSuccess);
            Assert.Equal(0, context.Tokens.Count());
        }

    }
}

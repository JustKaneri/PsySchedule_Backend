using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using Xunit.Abstractions;

namespace PsyScheduleTests.ServiceTests
{
    [Collection("Database")]
    public class RegisterTest : BaseTestClass, IClassFixture<PostgreSqlFixture>
    {
        private readonly ITestOutputHelper _outputHelper;

        public RegisterTest(PostgreSqlFixture fixture, ITestOutputHelper outputHelper) :base(fixture)  
        {
            _outputHelper = outputHelper;
        }

        /// <summary>
        /// Регистрация пользователя с корректными данными
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Registration_One_User()
        {
            using var scope = CreateScope();

            var contex = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IRegistrationService>();

            var regData = new RegisterPsychologistDto("Иван", "Иван123", "1234", "Russian Standard Time");
            var usData = new MetaDataDto("192.168.0.1", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

            var result = await service.RegisterPsychologistAsync(regData,usData , CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(1, contex.Psychologists.Count());
            Assert.Equal(1, contex.Tokens.Count());
        }

        /// <summary>
        /// Регистрация 2 пользователй с одинкавыми логинами, 
        /// с учетом race_condition
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Registration_Same_Users()
        {
            using var scope = CreateScope();
            using var scope2 = CreateScope();

            var service1 = scope.ServiceProvider.GetRequiredService<IRegistrationService>();
            var service2 = scope2.ServiceProvider.GetRequiredService<IRegistrationService>();

            var regData = new RegisterPsychologistDto("Иван", "Иван123", "1234", "Russian Standard Time");
            var regData2 = new RegisterPsychologistDto("Олег", "Иван123", "1234", "Russian Standard Time");
            var usData = new MetaDataDto("192.168.0.1", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

            var result1 = service1.RegisterPsychologistAsync(regData, usData, CancellationToken.None);
            var result2 = service2.RegisterPsychologistAsync(regData2, usData, CancellationToken.None);

            await result1;
            await result2;

            using var scopeVerification = CreateScope();

            var contex = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();
            _outputHelper.WriteLine(string.Join(" ",contex.Psychologists.Select(s => s.Name).ToList()));

            Assert.Equal(1, contex.Psychologists.Count());
            Assert.Equal(1, contex.Tokens.Count());
        }


        /// <summary>
        /// Регистрация пользователя с существующим логином
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Registration_Same_Users2()
        {
            using var scope = CreateScope();
            using var scope2 = CreateScope();

            var service1 = scope.ServiceProvider.GetRequiredService<IRegistrationService>();
            var service2 = scope2.ServiceProvider.GetRequiredService<IRegistrationService>();

            var regData = new RegisterPsychologistDto("Иван", "Иван123", "1234", "Russian Standard Time");
            var usData = new MetaDataDto("192.168.0.1", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

            var result1 = await service1.RegisterPsychologistAsync(regData, usData, CancellationToken.None);

            var regData2 = new RegisterPsychologistDto("Олег", "Иван123", "1234", "Russian Standard Time");

            var result2 = await service2.RegisterPsychologistAsync(regData2, usData, CancellationToken.None);

            using var scopeVerification = CreateScope();

            var contex = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            Assert.True(result1.IsSuccess);
            Assert.False(result2.IsSuccess);
            //Assert.Equal(400, result2.Error.errorCode);
            Assert.Equal("Иван", contex.Psychologists.First().Name);
            Assert.Equal(1, contex.Psychologists.Count());
            Assert.Equal(1, contex.Tokens.Count());
        }


        /// <summary>
        /// Регистрация 2 пользователей,
        /// Проверка что токены, соль, хэш паролей, Id разные
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Registration_Two_Different_Users()
        {
            using var scope = CreateScope();
            using var scope2 = CreateScope();

            var service1 = scope.ServiceProvider.GetRequiredService<IRegistrationService>();
            var service2 = scope2.ServiceProvider.GetRequiredService<IRegistrationService>();


            var regData = new RegisterPsychologistDto("Иван", "Иван123", "1234", "Russian Standard Time");
            var regData2 = new RegisterPsychologistDto("Олег", "Олег123", "1234", "Russian Standard Time");
            var usData = new MetaDataDto("192.168.0.1", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

            var result1 = await service1.RegisterPsychologistAsync(regData, usData, CancellationToken.None);
            var result2 = await service2.RegisterPsychologistAsync(regData2, usData, CancellationToken.None);


            using var scopeVerification = CreateScope();

            var contex = scopeVerification.ServiceProvider.GetRequiredService<DataContext>();

            Assert.True(result1.IsSuccess);
            Assert.True(result2.IsSuccess);

            Assert.NotEqual(result1.Value.AccessToken, result2.Value.AccessToken);
            Assert.NotEqual(result1.Value.RefreshToken, result2.Value.RefreshToken);

            Assert.Equal(2, contex.Psychologists.Count());
            Assert.Equal(2, contex.Tokens.Count());

            var users = await contex.Psychologists.ToListAsync();

            Assert.NotEqual(users[0].Id, users[1].Id);
            Assert.NotEqual(users[0].Salt, users[1].Salt);
            Assert.NotEqual(users[0].Password, users[1].Password);
        }
    }
}

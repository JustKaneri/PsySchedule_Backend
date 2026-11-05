using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PsySchedule.Context;
using PsySchedule.Dto;
using Xunit.Abstractions;
using PsySchedule.Interfaces;
using DtoRequest = PsySchedule.Dto.ClientTelegramRequest;
using DtoTelegramData = PsySchedule.Dto.TelegramData;
using PsySchedule.Models;

using GRPC = PsySchedule.GRPC;

namespace PsyScheduleTests.ServiceTests
{
    public class ClientTelegramServiceTest : BaseTestClass, IClassFixture<PostgreSqlFixture>
    {
        private readonly ITestOutputHelper _outputHelper;

        public ClientTelegramServiceTest(
            PostgreSqlFixture fixture,
            ITestOutputHelper outputHelper) : base(fixture)
        {
            _outputHelper = outputHelper;
        }

    /// <summary>
    /// Проверяет успешное создание нового клиента в базе данных.
    /// </summary>
        [Fact]
        public async Task Create_Client_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IClientService<DtoRequest>>();

            var request = new DtoRequest(
                "Ivan",
                "Ivanov",
                "+79001234567",
                "Europe/Moscow", 
                new DtoTelegramData(
                    123456L,
                    "ivan_test",
                    "Test",
                    "User",
                    "en",
                    123456L)
                );

            var result = await service.CreateAsync(request, CancellationToken.None);

            _outputHelper.WriteLine($"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            Assert.True(result.IsSuccess);

            using var resultScope = CreateScope();

            var resultContext = resultScope.ServiceProvider.GetRequiredService<DataContext>();

            var createdClient = await resultContext.Clients
                .FirstOrDefaultAsync(c => c.TelegramId == 123456L);

            Assert.NotNull(createdClient);
            Assert.Equal("Ivan", createdClient.FirstName);
            Assert.Equal("Ivanov", createdClient.SecondName);
            Assert.Equal("+79001234567", createdClient.Phone);
            Assert.Equal("Europe/Moscow", createdClient.TimeZone);
        }


    /// <summary>
    /// Проверяет, что при попытке создать клиента с уже существующим TelegramId регистрация пропускается.
    /// </summary>
        [Fact]
        public async Task Create_Client_For_Not_Exist_TelegramId_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IClientService<DtoRequest>>();

            // Сначала создаём клиента с ID 123456
            var existingClient = new Client
            {
                FirstName = "Existing",
                SecondName = "User",
                Phone = "+7900111222",
                TimeZone = "Europe/Moscow",
                Telegram = new ClientTelegram()
                {
                    Id = 123456L,
                    UserName ="ivan_test",
                    FirstName = "Test",
                    LastName = "User",
                    LanguageCode = "en",
                    TelegramChatId = 123456L
                }
            };

            context.Clients.Add(existingClient);
            await context.SaveChangesAsync();

            var request = new DtoRequest(
                "Ivan",
                "Ivanov",
                "+79001234567",
                "Europe/Moscow",
                new DtoTelegramData(
                    123456L,
                    "ivan_test",
                    "Test",
                    "User",
                    "en",
                    123456L)
                );

            var result = await service.CreateAsync(request, CancellationToken.None);

            _outputHelper.WriteLine(
                $"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            // Ожидаем успех (клиент уже существует, регистрация пропущена)
            Assert.True(result.IsSuccess);

            using var resultScope = CreateScope();

            var resultContext = resultScope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(1, await resultContext.Clients.CountAsync());
        }


    /// <summary>
    /// Проверяет создание нескольких клиентов с разными TelegramId.
    /// </summary>
        [Fact]
        public async Task Create_Multiple_Clients_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IClientService<DtoRequest>>();

            // Создаём первого клиента с ID 123456
            var request1 = new DtoRequest(
                "Ivan",
                "Ivanov",
                "+79001234567",
                "Europe/Moscow",
                new DtoTelegramData(
                    123456L,
                    "ivan_test",
                    "Test",
                    "User",
                    "en",
                    7890123456L)
                );

            var result1 = await service.CreateAsync(request1, CancellationToken.None);
            Assert.True(result1.IsSuccess);

            // Создаём второго клиента с другим ID
            var request2 = new DtoRequest(
                "Maria",
                "Petrova",
                "+79009876543",
                "Asia/Almaty",
                new DtoTelegramData(
                    789012L,
                    "maria_test",
                    "Maria",
                    "Petrova",
                    "ru",
                    7890123457L)
                );

            var result2 = await service.CreateAsync(request2, CancellationToken.None);
            Assert.True(result2.IsSuccess);

            using var resultScope = CreateScope();

            var resultContext = resultScope.ServiceProvider.GetRequiredService<DataContext>();

            var clients = await resultContext.Clients
                .ToListAsync();

            Assert.Equal(2, clients.Count);
        }


    /// <summary>
    /// Проверяет, что при повторной регистрации существующего клиента данные не обновляются.
    /// </summary>
        [Fact]
        public async Task Duplicate_Regions_Same_TelegramId_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IClientService<DtoRequest>>();

            // Создаём клиента с ID 123456
            var request1 = new DtoRequest(
                "Ivan",
                "Ivanov",
                "+79001234567",
                "Europe/Moscow",
                new DtoTelegramData(
                    123456L,
                    "ivan_test",
                    "Test",
                    "User",
                    "en",
                    7890123456L)
                );

            var result1 = await service.CreateAsync(request1, CancellationToken.None);
            Assert.True(result1.IsSuccess);

            // Попытка создать того же клиента с другими данными
            var request2 = new DtoRequest(
                "NewIvan",
                "NewIvanov",
                "+90012345678",
                "America/New_York",
                new DtoTelegramData(
                    123456L,
                    "new_ivan_test",
                    "NewIvan",
                    "NewIvanov",
                    "en",
                    7890123456L)
                );

            var result2 = await service.CreateAsync(request2, CancellationToken.None);

            // Ожидаем успех (клиент уже существует, данные не обновляются)
            Assert.True(result2.IsSuccess);

            using var resultScope = CreateScope();

            var resultContext = resultScope.ServiceProvider.GetRequiredService<DataContext>();

            var client = await resultContext.Clients
                .FirstOrDefaultAsync(c => c.TelegramId == 123456L);

            // Данные должны остаться прежними
            Assert.Equal("Ivan", client.FirstName);
            Assert.Equal("Ivanov", client.SecondName);
            Assert.Equal("+79001234567", client.Phone);
            Assert.Equal("Europe/Moscow", client.TimeZone);
        }

    }
}
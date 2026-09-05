using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using Xunit.Abstractions;

namespace PsyScheduleTests.ServiceTests
{
    public class ServiceManagerTest : BaseTestClass, IClassFixture<PostgreSqlFixture>
    {
        private readonly ITestOutputHelper _outputHelper;

        public ServiceManagerTest(
            PostgreSqlFixture fixture,
            ITestOutputHelper outputHelper) : base(fixture)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task Create_Service_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var psy = await context.Psychologists.FirstAsync();

            var request = new CreateServiceRequest(
                "Консультация",
                3000
            );

            var result = await service.CreateAsync(psy.Id, request, CancellationToken.None);

            _outputHelper.WriteLine($"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            Assert.True(result.IsSuccess);

            using var resultScope = CreateScope();

            var resultContext = resultScope.ServiceProvider.GetRequiredService<DataContext>();

            var createdService = await resultContext.Services.FirstOrDefaultAsync(s => s.PsychologistId == psy.Id);

            Assert.NotNull(createdService);
            Assert.Equal("Консультация", createdService.Name);
            Assert.Equal(3000, createdService.Price);
            Assert.Equal(psy.Id, createdService.PsychologistId);
            Assert.Equal(1, createdService.Version);
        }


        [Fact]
        public async Task Create_Service_For_Not_Exist_Psychologist_Test()
        {
            using var scope = CreateScope();

            var service =
                scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var request = new CreateServiceRequest(
                "Консультация",
                3000
            );

            var result = await service.CreateAsync(
                999999,
                request,
                CancellationToken.None);

            _outputHelper.WriteLine(
                $"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.Error?.ErrorCode);
            Assert.Equal("Психолог не найден", result.Error?.ErrorMessage);

            using var resultScope = CreateScope();

            var context =
                resultScope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(0, await context.Services.CountAsync());
        }


        [Fact]
        public async Task Create_Multiple_Services_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var psy = await context.Psychologists.FirstAsync();

            var request1 = new CreateServiceRequest(
                "Консультация",
                3000);

            var request2 = new CreateServiceRequest(
                "Семейная консультация",
                5000);

            var result1 = await service.CreateAsync(
                psy.Id,
                request1,
                CancellationToken.None);

            var result2 = await service.CreateAsync(
                psy.Id,
                request2,
                CancellationToken.None);

            Assert.True(result1.IsSuccess);
            Assert.True(result2.IsSuccess);

            using var resultScope = CreateScope();

            var resultContext =
                resultScope.ServiceProvider.GetRequiredService<DataContext>();

            var services = await resultContext.Services
                .Where(s => s.PsychologistId == psy.Id)
                .ToListAsync();

            Assert.Equal(2, services.Count);
        }


        [Fact]
        public async Task Get_Services_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var psy = await context.Psychologists.FirstAsync();

            var request1 = new CreateServiceRequest(
                "Консультация",
                3000);

            var request2 = new CreateServiceRequest(
                "Семейная консультация",
                5000);

            await service.CreateAsync(
                psy.Id,
                request1,
                CancellationToken.None);

            await service.CreateAsync(
                psy.Id,
                request2,
                CancellationToken.None);

            using var getScope = CreateScope();

            var getter =
                getScope.ServiceProvider.GetRequiredService<IServiceManager>();

            var result = await getter.GetServicesAsync(
                psy.Id,
                CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count());
        }


        [Fact]
        public async Task Get_Empty_Services_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var psy = await context.Psychologists.FirstAsync();

            var result = await service.GetServicesAsync(
                psy.Id,
                CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }


        [Fact]
        public async Task Delete_Service_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var psy = await context.Psychologists.FirstAsync();

            var createResult = await service.CreateAsync(
                psy.Id,
                new CreateServiceRequest("Консультация", 3000),
                CancellationToken.None);

            Assert.True(createResult.IsSuccess);

            var createdService = await context.Services
                .FirstAsync(s => s.PsychologistId == psy.Id);

            var result = await service.DeleteAsync(
                psy.Id,
                createdService.Id,
                CancellationToken.None);

            _outputHelper.WriteLine(
                $"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            Assert.True(result.IsSuccess);

            using var resultScope = CreateScope();

            var resultContext =
                resultScope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.False(
                await resultContext.Services
                    .AnyAsync(s => s.Id == createdService.Id));
        }


        [Fact]
        public async Task Delete_Not_Exist_Service_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var service =
                scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var result = await service.DeleteAsync(
                1,
                999999,
                CancellationToken.None);

            _outputHelper.WriteLine(
                $"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.Error?.ErrorCode);
            Assert.Equal(
                "Услуга 999999 не найдена",
                result.Error?.ErrorMessage);
        }


        [Fact]
        public async Task Delete_Service_Of_Another_Psychologist_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var psychologists = await context.Psychologists
                .OrderBy(p => p.Id)
                .Take(2)
                .ToListAsync();

            if (psychologists.Count < 2)
            {
                return;
            }

            var owner = psychologists[0];
            var anotherPsy = psychologists[1];

            await service.CreateAsync(
                owner.Id,
                new CreateServiceRequest("Консультация", 3000),
                CancellationToken.None);

            var createdService = await context.Services
                .FirstAsync(s => s.PsychologistId == owner.Id);

            var result = await service.DeleteAsync(
                anotherPsy.Id,
                createdService.Id,
                CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(403, result.Error?.ErrorCode);
            Assert.Equal(
                "У вас нет прав для удаления данной услуги",
                result.Error?.ErrorMessage);

            Assert.True(
                await context.Services
                    .AnyAsync(s => s.Id == createdService.Id));
        }


        [Fact]
        public async Task Update_Service_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var psy = await context.Psychologists.FirstAsync();

            await service.CreateAsync(
                psy.Id,
                new CreateServiceRequest("Консультация", 3000),
                CancellationToken.None);

            var createdService = await context.Services
                .FirstAsync(s => s.PsychologistId == psy.Id);

            var request = new UpdateServiceRequest(
                createdService.Id,
                "Новая консультация",
                5000,
                createdService.Version);

            var result = await service.UpdateAsync(
                psy.Id,
                request,
                CancellationToken.None);

            _outputHelper.WriteLine(
                $"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            Assert.True(result.IsSuccess);

            using var resultScope = CreateScope();

            var resultContext =
                resultScope.ServiceProvider.GetRequiredService<DataContext>();

            var updatedService = await resultContext.Services
                .FirstAsync(s => s.Id == createdService.Id);

            Assert.Equal("Новая консультация", updatedService.Name);
            Assert.Equal(5000, updatedService.Price);
            Assert.Equal(2, updatedService.Version);
        }


        [Fact]
        public async Task Update_Not_Exist_Service_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var service =
                scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var request = new UpdateServiceRequest(
                999999,
                "Новая консультация",
                5000,
                1);

            var result = await service.UpdateAsync(
                1,
                request,
                CancellationToken.None);

            _outputHelper.WriteLine(
                $"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.Error?.ErrorCode);
            Assert.Equal(
                "Услуга 999999 не найдена",
                result.Error?.ErrorMessage);
        }


        [Fact]
        public async Task Update_Service_Of_Another_Psychologist_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var psychologists = await context.Psychologists
                .OrderBy(p => p.Id)
                .Take(2)
                .ToListAsync();

            if (psychologists.Count < 2)
            {
                return;
            }

            var owner = psychologists[0];
            var anotherPsy = psychologists[1];

            await service.CreateAsync(
                owner.Id,
                new CreateServiceRequest("Консультация", 3000),
                CancellationToken.None);

            var createdService = await context.Services
                .FirstAsync(s => s.PsychologistId == owner.Id);

            var request = new UpdateServiceRequest(
                createdService.Id,
                "Попытка изменения",
                9999,
                createdService.Version);

            var result = await service.UpdateAsync(
                anotherPsy.Id,
                request,
                CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(403, result.Error?.ErrorCode);
            Assert.Equal(
                "У вас нет прав для удаления данной услуги",
                result.Error?.ErrorMessage);

            var unchangedService = await context.Services
                .FirstAsync(s => s.Id == createdService.Id);

            Assert.Equal("Консультация", unchangedService.Name);
            Assert.Equal(3000, unchangedService.Price);
            Assert.Equal(1, unchangedService.Version);
        }


        [Fact]
        public async Task Update_Service_With_Wrong_Version_Test()
        {
            await Registration();

            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

            var psy = await context.Psychologists.FirstAsync();

            await service.CreateAsync(
                psy.Id,
                new CreateServiceRequest("Консультация", 3000),
                CancellationToken.None);

            var createdService = await context.Services
                .FirstAsync(s => s.PsychologistId == psy.Id);

            var request = new UpdateServiceRequest(
                createdService.Id,
                "Новая консультация",
                5000,
                999);

            var result = await service.UpdateAsync(
                psy.Id,
                request,
                CancellationToken.None);

            _outputHelper.WriteLine(
                $"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            Assert.False(result.IsSuccess);
            Assert.Equal(409, result.Error?.ErrorCode);
            Assert.Equal(
                "Услуга была изменена другим запросом. Обновите данные и попробуйте снова",
                result.Error?.ErrorMessage);

            var unchangedService = await context.Services
                .FirstAsync(s => s.Id == createdService.Id);

            Assert.Equal("Консультация", unchangedService.Name);
            Assert.Equal(3000, unchangedService.Price);
            Assert.Equal(1, unchangedService.Version);
        }


        [Fact]
        public async Task Update_Service_Twice_With_Correct_Version_Test()
        {
            await Registration();

            int psyId;
            int serviceId;

            // Создание
            using (var scope = CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

                var psy = await context.Psychologists.FirstAsync();
                psyId = psy.Id;

                await service.CreateAsync(
                    psyId,
                    new CreateServiceRequest("Консультация", 3000),
                    CancellationToken.None);

                var createdService = await context.Services
                    .FirstAsync(s => s.PsychologistId == psyId);

                serviceId = createdService.Id;
                Assert.Equal(1, createdService.Version);
            }

            // Первый Update: 1 -> 2
            using (var scope = CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

                var request = new UpdateServiceRequest(
                    serviceId,
                    "Первая версия",
                    4000,
                    1);

                var result = await service.UpdateAsync(
                    psyId,
                    request,
                    CancellationToken.None);

                Assert.True(result.IsSuccess);
            }

            // Второй Update: 2 -> 3
            using (var scope = CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var service = scope.ServiceProvider.GetRequiredService<IServiceManager>();

                // Проверяем, что из БД действительно пришла версия 2
                var currentService = await context.Services
                    .AsNoTracking()
                    .FirstAsync(s => s.Id == serviceId);

                Assert.Equal(2, currentService.Version);

                var request = new UpdateServiceRequest(
                    serviceId,
                    "Вторая версия",
                    5000,
                    2);

                var result = await service.UpdateAsync(
                    psyId,
                    request,
                    CancellationToken.None);

                Assert.True(result.IsSuccess);
            }

            // Проверяем результат
            using (var scope = CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                var updatedService = await context.Services
                    .AsNoTracking()
                    .FirstAsync(s => s.Id == serviceId);

                Assert.Equal("Вторая версия", updatedService.Name);
                Assert.Equal(5000, updatedService.Price);
                Assert.Equal(3, updatedService.Version);
            }
        }

        [Fact]
        public async Task Update_Service_With_Old_Version_After_Successful_Update_Test()
        {
            await Registration();

            int psyId;
            int serviceId;

            // Создаём услугу
            using (var scope = CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var serviceManager = scope.ServiceProvider.GetRequiredService<IServiceManager>();

                var psy = await context.Psychologists.FirstAsync();
                psyId = psy.Id;

                var createResult = await serviceManager.CreateAsync(
                    psyId,
                    new CreateServiceRequest("Консультация", 3000),
                    CancellationToken.None);

                Assert.True(createResult.IsSuccess);

                var createdService = await context.Services
                    .AsNoTracking()
                    .FirstAsync(s =>
                        s.PsychologistId == psyId &&
                        s.Name == "Консультация");

                serviceId = createdService.Id;

                Assert.Equal(1, createdService.Version);
            }

            // Первое успешное обновление: Version 1 -> 2
            using (var scope = CreateScope())
            {
                var serviceManager = scope.ServiceProvider
                    .GetRequiredService<IServiceManager>();

                var result = await serviceManager.UpdateAsync(
                    psyId,
                    new UpdateServiceRequest(
                        serviceId,
                        "Обновлённая консультация",
                        4000,
                        1),
                    CancellationToken.None);

                Assert.True(result.IsSuccess);
            }

            // Проверяем, что версия действительно стала 2
            using (var scope = CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                var service = await context.Services
                    .AsNoTracking()
                    .FirstAsync(s => s.Id == serviceId);

                Assert.Equal("Обновлённая консультация", service.Name);
                Assert.Equal(4000, service.Price);
                Assert.Equal(2, service.Version);
            }

            // Пытаемся обновить услугу со старой версией 1
            using (var scope = CreateScope())
            {
                var serviceManager = scope.ServiceProvider
                    .GetRequiredService<IServiceManager>();

                var result = await serviceManager.UpdateAsync(
                    psyId,
                    new UpdateServiceRequest(
                        serviceId,
                        "Попытка обновления со старой версией",
                        5000,
                        1),
                    CancellationToken.None);

                Assert.False(result.IsSuccess);
                Assert.Equal(409, result.Error.ErrorCode);
            }

            // Проверяем, что данные не изменились
            using (var scope = CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                var service = await context.Services
                    .AsNoTracking()
                    .FirstAsync(s => s.Id == serviceId);

                Assert.Equal("Обновлённая консультация", service.Name);
                Assert.Equal(4000, service.Price);
                Assert.Equal(2, service.Version);
            }
        }

    }
}
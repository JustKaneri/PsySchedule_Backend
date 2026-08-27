using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PsySchedule.Context;
using PsySchedule.Dto;
using PsySchedule.Interfaces;
using PsySchedule.Models;
using PsySchedule.Services;
using Respawn;
using Serilog;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace PsyScheduleTests
{
    [Collection("Database")]
    public class BaseTestClass : IAsyncLifetime
    {
        protected readonly PostgreSqlFixture Fixture;
        protected readonly static RegisterPsychologistDto regData = new RegisterPsychologistDto("Иван", "Иван123", "1234", "Russian Standard Time");
        protected readonly static MetaDataDto usData = new MetaDataDto("192.168.0.1", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

        protected BaseTestClass(PostgreSqlFixture fixture)
        {
            Fixture = fixture;
        }

        public async Task InitializeAsync()
        {
            await Fixture.ResetDatabaseAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        protected IServiceScope CreateScope()
        {
            return Fixture._provider.CreateScope();
        }

        protected async Task<AuthTokensDto> Registration()
        {
            using var scope = CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IRegistrationService>();
            var reg = await service.RegisterPsychologistAsync(regData, usData, CancellationToken.None);

            return reg.Value;
        }
    }
}

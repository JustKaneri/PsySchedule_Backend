using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PsySchedule.Context;
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
    }
}

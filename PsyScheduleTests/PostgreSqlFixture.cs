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
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Testcontainers.PostgreSql;

namespace PsyScheduleTests
{
    public class PostgreSqlFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgres;

        public DataContext _context = null!;
        public ServiceProvider _provider = null!;
        private Respawner _respawner = null!;
        private DbConnection _connection = null!;

        public PostgreSqlFixture()
        {
            _postgres = new PostgreSqlBuilder()
                 .WithImage("postgres:18")
                 .WithDatabase("PsySchedule")
                 .WithUsername("postgres")
                 .WithPassword("postgres")
                 .Build();
        }

        public async Task InitializeAsync()
        {
            await _postgres.StartAsync();

            var services = new ServiceCollection();

            InitConfiguration(services);
            RegistrationServices(services);

            _provider = services.BuildServiceProvider();

            // Создаем таблицы
            await InitDataBaseAsync();
        }

        private void InitConfiguration(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.Configure<TokenParameters>(configuration.GetSection("Jwt"));
        }

        private void RegistrationServices(ServiceCollection services)
        {
            // Регистрируем DbContext
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(_postgres.GetConnectionString());
            });


            services.AddSerilog();
            services.AddLogging();

            // Регистрируем сервисы
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

        private async Task InitDataBaseAsync()
        {
            using var scope = _provider.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<DataContext>();

            await _context.Database.MigrateAsync();

            // Создаем отдельное соединение для Respawn
            _connection = new NpgsqlConnection(_postgres.GetConnectionString());
            await _connection.OpenAsync();

            _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres
            });
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_connection);
        }

        public async Task DisposeAsync()
        {
            await _connection.DisposeAsync();

            await _context.DisposeAsync();

            await _postgres.DisposeAsync();
        }
    }
}

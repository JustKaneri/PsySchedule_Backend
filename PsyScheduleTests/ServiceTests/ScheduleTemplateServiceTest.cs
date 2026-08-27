using Microsoft.EntityFrameworkCore;
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
    public class ScheduleTemplateServiceTest : BaseTestClass, IClassFixture<PostgreSqlFixture>
    {
        private readonly ITestOutputHelper _outputHelper;

        public ScheduleTemplateServiceTest(PostgreSqlFixture fixture, ITestOutputHelper outputHelper) : base(fixture)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task Create_Template_Test()
        {
            var reg = await Registration();

            using var scope = CreateScope();
            var psy = await (scope.ServiceProvider.GetRequiredService<DataContext>()).Psychologists.FirstAsync();
            var service = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var schedule = new ScheduleTemplateDayDto(1, new TimeRange("10:00", "19:00"),new TimeRange("12:00", "13:00"), 15);
            var schedules = new List<ScheduleTemplateDayDto>(){ schedule };

            var result = await service.CreateAsync(schedules, psy.Id, CancellationToken.None);

            _outputHelper.WriteLine($"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");
            Assert.True(result.IsSuccess);

            using var resultScope = CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(1, context.ScheduleTemplates.Count());
            Assert.Equal(2, context.WorkDays.Count());
        }

        [Fact]
        public async Task Create_Parallel_Template_Test()
        {
            var reg = await Registration();

            using var scope = CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();
            using var scope2 = CreateScope();
            var service2 = scope2.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var psy = await (scope.ServiceProvider.GetRequiredService<DataContext>()).Psychologists.FirstAsync();

            var schedule = new ScheduleTemplateDayDto(1, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15);
            var schedules = new List<ScheduleTemplateDayDto>() { schedule };

            var result = service.CreateAsync(schedules, psy.Id, CancellationToken.None);
            var result2 = service2.CreateAsync(schedules, psy.Id, CancellationToken.None);

            await result;
            await result2;

            _outputHelper.WriteLine($"Result 1 {result.Result.IsSuccess} Error {result.Result?.Error?.ErrorMessage}");
            _outputHelper.WriteLine($"Result 2 {result2.Result.IsSuccess} Error {result2.Result?.Error?.ErrorMessage}");
            Assert.True(result.Result.IsSuccess ^ result2.Result.IsSuccess);

            using var resultScope = CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(1, context.ScheduleTemplates.Count());
            Assert.Equal(2, context.WorkDays.Count());
        }

        [Fact]
        public async Task Create_Same_Template_Test()
        {
            var reg = await Registration();

            using var scope = CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();
            using var scope2 = CreateScope();
            var service2 = scope2.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var psy = await (scope.ServiceProvider.GetRequiredService<DataContext>()).Psychologists.FirstAsync();

            var schedule = new ScheduleTemplateDayDto(1, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15);
            var schedules = new List<ScheduleTemplateDayDto>() { schedule };

            var result = await service.CreateAsync(schedules, psy.Id, CancellationToken.None);
            var result2 = await service2.CreateAsync(schedules, psy.Id, CancellationToken.None);


            _outputHelper.WriteLine($"Result 1 {result.IsSuccess} Error {result.Error?.ErrorMessage}");
            _outputHelper.WriteLine($"Result 2 {result2.IsSuccess} Error {result2.Error?.ErrorMessage}");

            Assert.True(result.IsSuccess);
            Assert.False(result2.IsSuccess);

            using var resultScope = CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(1, context.ScheduleTemplates.Count());
            Assert.Equal(2, context.WorkDays.Count());
        }

        [Fact]
        public async Task Create_Template_For_Not_Exist_Ussr_Test()
        {
            using var scope = CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();


            var schedule = new ScheduleTemplateDayDto(1, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15);
            var schedules = new List<ScheduleTemplateDayDto>() { schedule };

            var result = await service.CreateAsync(schedules,1, CancellationToken.None);

            _outputHelper.WriteLine($"Result 1 {result.IsSuccess} Error {result.Error?.ErrorMessage}");

            Assert.False(result.IsSuccess);

            using var resultScope = CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(0, context.ScheduleTemplates.Count());
            Assert.Equal(0, context.WorkDays.Count());
        }


        [Fact]
        public async Task Create_Template_Full_Weekend_Test()
        {
            var reg = await Registration();

            using var scope = CreateScope();
            var psy = await (scope.ServiceProvider.GetRequiredService<DataContext>()).Psychologists.FirstAsync();
            var service = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var schedules = new List<ScheduleTemplateDayDto>() {
                new ScheduleTemplateDayDto(1, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(2, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(3, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(4, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(5, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(6, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(7, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15)};

            var result = await service.CreateAsync(schedules, psy.Id, CancellationToken.None);

            _outputHelper.WriteLine($"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");
            Assert.True(result.IsSuccess);

            using var resultScope = CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(7, context.ScheduleTemplates.Count());
            Assert.Equal(14, context.WorkDays.Count());
            Assert.Equal(14, context.WorkDays.Where(wd => wd.PsychologistId == psy.Id).Count());
        }

        [Fact]
        public async Task Create_Template_Half_Weekend_Test()
        {
            var reg = await Registration();

            using var scope = CreateScope();
            var psy = await (scope.ServiceProvider.GetRequiredService<DataContext>()).Psychologists.FirstAsync();
            var service = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var schedules = new List<ScheduleTemplateDayDto>() {
                new ScheduleTemplateDayDto(1, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(2, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(3, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(4, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15)};

            var result = await service.CreateAsync(schedules, psy.Id, CancellationToken.None);

            _outputHelper.WriteLine($"Result {result.IsSuccess} Error {result.Error?.ErrorMessage}");
            Assert.True(result.IsSuccess);

            using var resultScope = CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(4, context.ScheduleTemplates.Count());
            Assert.Equal(8, context.WorkDays.Count());
            Assert.Equal(8, context.WorkDays.Where(wd => wd.PsychologistId == psy.Id).Count());
        }

        [Fact]
        public async Task Get_Template_Test()
        {
            var reg = await Registration();
            using var scope = CreateScope();
            var psy = await (scope.ServiceProvider.GetRequiredService<DataContext>()).Psychologists.FirstAsync();
            var service = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var schedules = new List<ScheduleTemplateDayDto>() {
                new ScheduleTemplateDayDto(1, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(2, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(3, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15),
                new ScheduleTemplateDayDto(4, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15)};

            await service.CreateAsync(schedules, psy.Id, CancellationToken.None);


            using var scopeGet = CreateScope();
            var geter = scopeGet.ServiceProvider.GetRequiredService<IScheduleTemplateService>();


            var result = await geter.GetAsync(psy.Id,CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(4, result.Value.TemplateDays.Count);
        }

        [Fact]
        public async Task Get_Empty_Template_Test()
        {
            var reg = await Registration();
            using var scope = CreateScope();
            var psy = await (scope.ServiceProvider.GetRequiredService<DataContext>()).Psychologists.FirstAsync();

            using var scopeGet = CreateScope();
            var geter = scopeGet.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var result = await geter.GetAsync(psy.Id, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.Error.ErrorCode);
        }

        [Fact]
        public async Task Update_Template_Test()
        {
            var reg = await Registration();

            using var scope = CreateScope();
            var psy = await (scope.ServiceProvider.GetRequiredService<DataContext>()).Psychologists.FirstAsync();
            var service = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var schedule = new ScheduleTemplateDayDto(1, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15);
            var schedules = new List<ScheduleTemplateDayDto>() { schedule };

            await service.CreateAsync(schedules, psy.Id, CancellationToken.None);


            schedule = schedule with { WorkTime = new TimeRange("11:00", "20:00") };

            using var scope2 = CreateScope();
            var serviceUpdater = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var resultUpd = await serviceUpdater.UpdateOrCreateAsync(schedule, psy.Id, CancellationToken.None);

            Assert.True(resultUpd.IsSuccess);


            using var resultScope = CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(TimeOnly.Parse("11:00"), context.ScheduleTemplates.First().StartedAt);
            Assert.Equal(TimeOnly.Parse("20:00"), context.ScheduleTemplates.First().FinishedAt);
        }

        [Fact]
        public async Task Update_Not_Exist_Template_Test()
        {
            var reg = await Registration();

            using var scope = CreateScope();
            var psy = await (scope.ServiceProvider.GetRequiredService<DataContext>()).Psychologists.FirstAsync();
            var service = scope.ServiceProvider.GetRequiredService<IScheduleTemplateService>();

            var schedule = new ScheduleTemplateDayDto(1, new TimeRange("10:00", "19:00"), new TimeRange("12:00", "13:00"), 15);

            var resultUpd = await service.UpdateOrCreateAsync(schedule, psy.Id, CancellationToken.None);

            Assert.True(resultUpd.IsSuccess);

            using var resultScope = CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            Assert.Equal(1, context.ScheduleTemplates.Count());
            Assert.Equal(2, context.WorkDays.Count());
        }
    }
}

using PsySchedule.Dto;
using PsySchedule.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsyScheduleTests.ValidationsTests
{
    public class ScheduleTemplateDayDtoValidatorTest
    {
        public readonly ScheduleTemplateDayDtoValidator _validator;

        public ScheduleTemplateDayDtoValidatorTest()
        {
            _validator = new ScheduleTemplateDayDtoValidator();
        }

        [Fact]
        public void Correct_Data_Test()
        {
            ScheduleTemplateDayDto schedule = new ScheduleTemplateDayDto(1,
                                                                         new TimeRange("10:00","19:00"),
                                                                         new TimeRange("12:00","13:00"),
                                                                         15);

            var result = _validator.Validate(schedule);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Not_Correct_WeekDay_Test()
        {
            ScheduleTemplateDayDto schedule = new ScheduleTemplateDayDto(10,
                                                                         new TimeRange("10:00", "19:00"),
                                                                         new TimeRange("12:00", "13:00"),
                                                                         15);

            var result = _validator.Validate(schedule);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Not_Correct_Gap_Test()
        {
            ScheduleTemplateDayDto schedule = new ScheduleTemplateDayDto(2,
                                                                         new TimeRange("10:00", "19:00"),
                                                                         new TimeRange("12:00", "13:00"),
                                                                         120);

            var result = _validator.Validate(schedule);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Break_Out_Work_Time_Test()
        {
            ScheduleTemplateDayDto schedule = new ScheduleTemplateDayDto(2,
                                                                         new TimeRange("10:00", "19:00"),
                                                                         new TimeRange("9:00", "11:00"),
                                                                         20);

            var result = _validator.Validate(schedule);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Break_Out_Work_Time_Test2()
        {
            ScheduleTemplateDayDto schedule = new ScheduleTemplateDayDto(2,
                                                                         new TimeRange("10:00", "19:00"),
                                                                         new TimeRange("10:00", "11:00"),
                                                                         20);

            var result = _validator.Validate(schedule);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Break_Out_Work_Time_Test3()
        {
            ScheduleTemplateDayDto schedule = new ScheduleTemplateDayDto(2,
                                                                         new TimeRange("10:00", "19:00"),
                                                                         new TimeRange("11:00", "20:00"),
                                                                         20);

            var result = _validator.Validate(schedule);

            Assert.False(result.IsValid);
        }


        [Fact]
        public void Break_Out_Work_Time_Test4()
        {
            ScheduleTemplateDayDto schedule = new ScheduleTemplateDayDto(2,
                                                                         new TimeRange("10:00", "19:00"),
                                                                         new TimeRange("20:00", "21:00"),
                                                                         20);

            var result = _validator.Validate(schedule);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Break_Out_Work_Time_Test5()
        {
            ScheduleTemplateDayDto schedule = new ScheduleTemplateDayDto(2,
                                                                         new TimeRange("10:00", "19:00"),
                                                                         new TimeRange("18:00", "19:00"),
                                                                         20);

            var result = _validator.Validate(schedule);

            Assert.True(result.IsValid);
        }

    }
}

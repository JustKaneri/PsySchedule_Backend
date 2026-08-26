using PsySchedule.Dto;
using PsySchedule.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsyScheduleTests.ValidationsTests
{
    public class ScheduleTemplatesValidatorTest
    {
        private readonly ScheduleTemplatesValidator _validator;

        public ScheduleTemplatesValidatorTest()
        {
            _validator = new ScheduleTemplatesValidator();
        }

        [Fact]
        public void Correct_Data_Test()
        {
            ScheduleTemplateDayDto schedule = new ScheduleTemplateDayDto(1,
                                                             new TimeRange("10:00", "19:00"),
                                                             new TimeRange("12:00", "13:00"),
                                                             15);
            List<ScheduleTemplateDayDto> days = new List<ScheduleTemplateDayDto>();
            days.Add(schedule);

            var result = _validator.Validate(days);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Correct_Data_Test2()
        {

            List<ScheduleTemplateDayDto> days = new List<ScheduleTemplateDayDto>();

            for (int i = 1; i < 8; i++)
            {
                days.Add(new ScheduleTemplateDayDto(i,
                                                     new TimeRange("10:00", "19:00"),
                                                     new TimeRange("12:00", "13:00"),
                                                     15));
            }

            var result = _validator.Validate(days);

            Assert.True(result.IsValid);
        }


        [Fact]
        public void Empty_List_Test()
        {

            List<ScheduleTemplateDayDto> days = new List<ScheduleTemplateDayDto>();

            var result = _validator.Validate(days);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Same_Elements_Test()
        {

            List<ScheduleTemplateDayDto> days = new List<ScheduleTemplateDayDto>();

            for (int i = 1; i < 8; i++)
            {
                days.Add(new ScheduleTemplateDayDto(1,
                                                     new TimeRange("10:00", "19:00"),
                                                     new TimeRange("12:00", "13:00"),
                                                     15));
            }

            var result = _validator.Validate(days);

            Assert.False(result.IsValid);
        }
    }
}

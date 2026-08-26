using PsySchedule.Dto;
using PsySchedule.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsyScheduleTests.ValidationsTests
{
    public class TimeRangeValidatorTest
    {
        public readonly TimeRangeValidator _validations;

        public TimeRangeValidatorTest()
        {
              _validations = new TimeRangeValidator();
        }

        [Fact]
        public void Valid_Data_Test()
        {
            TimeRange time = new TimeRange("10:00", "12:00");

            var result = _validations.Validate(time);
              
            Assert.True(result.IsValid);
        }


        [Fact]
        public void Not_Correct_Format_Test()
        {
            TimeRange time = new TimeRange("10.00", "12_00");

            var result = _validations.Validate(time);

            Assert.False(result.IsValid);
        }


        [Fact]
        public void Empty_Value_Test()
        {
            TimeRange time = new TimeRange("", "");

            var result = _validations.Validate(time);

            Assert.False(result.IsValid);
        }


        [Fact]
        public void End_Less_Start_Test()
        {
            TimeRange time = new TimeRange("19:00", "12:00");

            var result = _validations.Validate(time);

            Assert.False(result.IsValid);
        }
    }
}

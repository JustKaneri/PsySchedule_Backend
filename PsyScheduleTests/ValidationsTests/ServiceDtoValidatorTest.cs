using PsySchedule.Dto;
using PsySchedule.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsyScheduleTests.ValidationsTests
{
    public class ServiceDtoValidatorTest
    {
        private readonly ServiceDtoValidator _validator;

        public ServiceDtoValidatorTest()
        {
            _validator = new ServiceDtoValidator();
        }

        [Fact]
        public void Correct_Test()
        {
            CreateServiceRequest request = new CreateServiceRequest("Консульация", 500);

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Empty_Name_Test()
        {
            CreateServiceRequest request = new CreateServiceRequest("", 500);

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Zero_Price_Test()
        {
            CreateServiceRequest request = new CreateServiceRequest("Консультация", 0);

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Price_On_Bord_Test()
        {
            CreateServiceRequest request = new CreateServiceRequest("Консультация", 500_000);

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Price_Less_Zero_Test()
        {
            CreateServiceRequest request = new CreateServiceRequest("Консультация", -1);

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}

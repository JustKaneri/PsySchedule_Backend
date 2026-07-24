using PsySchedule.Dto;
using PsySchedule.Models;
using PsySchedule.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsyScheduleTests.ValidationsTests
{
    public class PsychologistRegistrationValidatorTest
    {
        private readonly PsychologistRegistrationValidator validations;

        public PsychologistRegistrationValidatorTest()
        {
            validations = new PsychologistRegistrationValidator();
        }

        [Fact]
        public void Valid_Data()
        {
            RegisterPsychologistDto registerPsychologist = new RegisterPsychologistDto(
                "bob123", "bob123", "A2345678a!", "Russian Standard Time");

            var result = validations.Validate(registerPsychologist);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Not_Valid_Name()
        {
            RegisterPsychologistDto registerPsychologist = new RegisterPsychologistDto(
                "222", "bob123", "A2345678a!", "Russian Standard Time");

            var result = validations.Validate(registerPsychologist);

            Assert.False(result.IsValid);
            Assert.Equal(1, result.Errors.Count());
            Assert.Equal("Длина имени должна быть от 6 символов", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Null_Parametrs()
        {
            RegisterPsychologistDto registerPsychologist = new RegisterPsychologistDto(
                null, null, null, null);

            var result = validations.Validate(registerPsychologist);

            Assert.False(result.IsValid);
            Assert.Equal(5, result.Errors.Count());

            Assert.Equal("Имя не может быть пустым", result.Errors[0].ErrorMessage);
            Assert.Equal("Логин не может быть пустым", result.Errors[1].ErrorMessage);
            Assert.Equal("Пароль не может быть пустым", result.Errors[2].ErrorMessage);
            Assert.Equal("Часовой пояс не может быть пустым", result.Errors[3].ErrorMessage);
            Assert.Equal("Некорректный часовой пояс", result.Errors[4].ErrorMessage );
        }

        [Fact]
        public void Short_Passowrd()
        {
            RegisterPsychologistDto shortPassword = new RegisterPsychologistDto(
                "bob123", "bob123", "12345", "Russian Standard Time");

            var result = validations.Validate(shortPassword);

            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count());
            Assert.Equal("Пароль должен быть от 8 символов", result.Errors[0].ErrorMessage);
            
        }

        [Fact]
        public void Long_Password()
        {
            RegisterPsychologistDto longPassword = new RegisterPsychologistDto(
                    "bob123", "bob123", new string('a',100), "Russian Standard Time");

            var result = validations.Validate(longPassword);

            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count());
            Assert.Equal("Длина пароля должна быть до 32 символов", result.Errors[0].ErrorMessage);
        }


        [Fact]
        public void Regex_Password()
        {
            RegisterPsychologistDto longPassword = new RegisterPsychologistDto(
                    "bob123", "bob123", "12345678", "Russian Standard Time");

            var result = validations.Validate(longPassword);

            Assert.False(result.IsValid);
            Assert.Equal(1, result.Errors.Count());
            Assert.Equal("Пароль должен содержать хотя бы одну цифру, одну строчную и одну заглавную букву", result.Errors[0].ErrorMessage);
        }


        [Fact]
        public void Not_Correct_Time_Zone()
        {
            RegisterPsychologistDto data = new RegisterPsychologistDto(
                    "bob123", "bob123", "A2345678a!", "Rus Standard Time");

            var result = validations.Validate(data);

            Assert.False(result.IsValid);
            Assert.Equal(1, result.Errors.Count());
            Assert.Equal("Некорректный часовой пояс", result.Errors[0].ErrorMessage);
        }
    }
}

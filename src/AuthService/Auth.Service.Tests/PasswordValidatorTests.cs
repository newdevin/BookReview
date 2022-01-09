using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Auth.Service.Tests
{
    public class PasswordValidatorTests
    {
        PasswordValidator _passwordValidator;
        public PasswordValidatorTests()
        {
            _passwordValidator = new PasswordValidator();
        }

        [Theory]
        [InlineData("1 missing uppercase letter", "1 missing uppercase letter",5, PasswordValidator.Password_Is_Missing_Capital_Letter)]
        [InlineData("1 MISSING LOWERCASE LETTER", "1 MISSING LOWERCASE LETTER", 5, PasswordValidator.Password_Is_Missing_Small_Letter)]
        [InlineData("Missing a number", "Missing a number", 5, PasswordValidator.Password_Is_Missing_Number)]
        [InlineData("1 Password", "2 Password", 5, PasswordValidator.RepeatPassword_Does_Not_Match)]
        [InlineData("", "", 5, PasswordValidator.Password_Is_Empty)]
        [InlineData(null, null, 5, PasswordValidator.Password_Is_Empty)]
        public void Validate_ShouldReturnErrorMessageWhenPasswordDoNotMeetCriteria(string password, string repeatPassword, int minPasswordLength, string message)
        {
            var result = _passwordValidator.Validate(password,repeatPassword, minPasswordLength);

            result.Should().NotBeNullOrEmpty();
            result.Should().Contain(message);
        }

        [Fact]
        public void Validate_ShouldReturnErrorMessageWhenPasswordIsNotCorrectLength()
        {
            string password = "1 Password";
            string repeatPassword = "1 Password";
            int minPasswordLength = 12;
            string message = string.Format(PasswordValidator.Password_Not_Long_Enough, minPasswordLength);

            var result = _passwordValidator.Validate(password, repeatPassword, minPasswordLength);

            result.Should().NotBeNullOrEmpty();
            result.Should().Contain(message);
        }

        [Fact]
        public void Validate_ShouldReturnNoErrorMessages()
        {
            string password = "1 Password";
            string repeatPassword = "1 Password";
            int minPasswordLength = 5;
            
            var result = _passwordValidator.Validate(password, repeatPassword, minPasswordLength);

            result.Should().BeEmpty();
        }
    }

}

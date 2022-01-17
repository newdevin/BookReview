using FluentAssertions;
using Xunit;

namespace BookReview.Service.Tests
{
    public class EmailValidationTests
    { 
        [Theory]
        [InlineData("someone@abc.com", true)]
        [InlineData("someone@abc.", false)]
        [InlineData("someoneabc.com", false)]
        [InlineData("someone", false)]
        [InlineData("@", false)]
        [InlineData("", false)]
        [InlineData("  ", false)]
        [InlineData(null, false)]
        public void Validate_ShouldReturnCorrectResult(string email, bool expected)
        {
            EmailValidator validator = new();
            var actual = validator.Validate(email);
            actual.Should().Be(expected);
        }
    }

}

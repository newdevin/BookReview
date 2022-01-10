using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Auth.Domain;
using FluentAssertions;
namespace Auth.Domain.Tests
{
    public class UserTests
    {
        [Fact]
        public void UpdatePassword_ShouldUpdateTheUserCorrectly()
        {
            User user = new("someone@abc.com", "first name", "last name", false,"salt", "hash", DateTime.UtcNow, DateTime.UtcNow);
            var newHash = "new hash";
            var newSalt = "new salt";
            var originalLastModifiedDateTimeInUtc = user.LastModifiedDateTimeUtc;

            var result = user.UpdatePassword(newSalt, newHash);

            result.Should().NotBeNull();
            result.PasswordSalt.Should().Be(newSalt);
            result.PasswordHash.Should().Be(newHash);
            result.LastModifiedDateTimeUtc.Should().NotBe(originalLastModifiedDateTimeInUtc);
        }

        [Fact]
        public void UpdateName_ShouldUpdateTheUserCorrectly()
        {
            User user = new("someone@abc.com", "first name", "last name", false, "salt", "hash", DateTime.UtcNow, DateTime.UtcNow);
            var newFirstName = "new first name";
            var newLastName = "new last name";
            var originalLastModifiedDateTimeInUtc = user.LastModifiedDateTimeUtc;

            var result = user.UpdateName(newFirstName, newLastName);

            result.Should().NotBeNull();
            result.FirstName.Should().Be(newFirstName);
            result.LastName.Should().Be(newLastName);
            result.LastModifiedDateTimeUtc.Should().NotBe(originalLastModifiedDateTimeInUtc);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void VerifyEmail_ShouldUpdateTheUserCorrectly(bool verified)
        {
            User user = new("someone@abc.com", "first name", "last name", false, "salt", "hash", DateTime.UtcNow, DateTime.UtcNow);
            var originalLastModifiedDateTimeInUtc = user.LastModifiedDateTimeUtc;

            var result = user.VerifyEmail(verified);

            result.Should().NotBeNull();
            result.EmailVerified.Should().Be(verified);
            result.LastModifiedDateTimeUtc.Should().NotBe(originalLastModifiedDateTimeInUtc);
        }
    }
}

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
        public void UpadateUser_ShouldUpdateTheUserCorrectly()
        {
            User user = new("someone@abc.com", "salt", "hash", DateTime.UtcNow, DateTime.UtcNow);
            var newHash = "new hash";
            var newSalt = "new salt";
            var originalLastModifiedDateTimeInUtc = user.LastModifiedDateTimeUtc;

            var result = user.UpdatePassword(newSalt, newHash);

            result.Should().NotBeNull();
            result.PasswordSalt.Should().Be(newSalt);
            result.PasswordHash.Should().Be(newHash);
            result.LastModifiedDateTimeUtc.Should().NotBe(originalLastModifiedDateTimeInUtc);


        }
    }
}

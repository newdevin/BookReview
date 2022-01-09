using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain
{
    public record User(string Email, string FirstName, string LastName, string PasswordSalt, string PasswordHash, DateTime CreatedDateTimeUtc, DateTime LastModifiedDateTimeUtc)
    {
        public User UpdatePassword(string passwordSalt, string passwordHash)
        {
            return this with { LastModifiedDateTimeUtc = DateTime.UtcNow, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
        }

        public User UpdateName(string FirstName, string LastName)
        {
            return this with { FirstName = FirstName, LastName = LastName, LastModifiedDateTimeUtc = DateTime.UtcNow };
        }
    }
}

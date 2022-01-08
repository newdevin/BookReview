using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain
{
    public record User(string Email, string PasswordSalt, string PasswordHash, DateTime CreatedDateTimeUtc, DateTime LastModifiedDateTimeUtc)
    {
        public User UpdatePassword(string passwordSalt, string passwordHash)
        {
            return this with { LastModifiedDateTimeUtc = DateTime.UtcNow, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
        }
    }
}

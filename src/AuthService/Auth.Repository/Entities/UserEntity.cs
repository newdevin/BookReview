using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Repository.Entities
{
    internal class UserEntity
    {
        [ExplicitKey]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime LastModifiedDateTimeUtc { get; set; }
    }
}

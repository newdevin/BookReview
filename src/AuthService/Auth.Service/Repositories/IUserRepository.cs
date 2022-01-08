using Auth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Repositories
{
    public interface IUserRepository
    {
        Task<User> InsertUser(User user);
        Task<User> Get(string email);
    }
}

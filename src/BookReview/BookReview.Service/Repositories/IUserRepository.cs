using BookReview.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.Service.Repositories
{
    public interface IUserRepository
    {
        Task<User> Insert(User user);
        Task<User> Get(string email);
        Task Update(User user);
    }

}

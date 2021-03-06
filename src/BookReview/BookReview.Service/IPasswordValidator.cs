using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.Service;

public interface IPasswordValidator
{
    IEnumerable<string> Validate(string password, string repeatPassword, int minPasswordLength);
}

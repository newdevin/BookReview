using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service
{
    public  interface IEncryptor
    {
        string GenerateSalt();
        string ComputeHash(string password, string salt);
    }
}

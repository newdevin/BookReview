using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service
{
    public record Result<T> (T Value, string Message, List<string> ErrorMessages);
    
}

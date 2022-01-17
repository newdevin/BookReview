using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.Service
{
    public record Result<T> (T? Value, string? Message, IEnumerable<string> ErrorMessages);
    
}

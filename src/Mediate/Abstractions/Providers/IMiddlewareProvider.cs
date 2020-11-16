using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    public interface IMiddlewareProvider : IEventMiddlewareProvider, IQueryMiddlewareProvider
    {
    }
}

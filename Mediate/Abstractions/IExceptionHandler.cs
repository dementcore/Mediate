using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    public interface IExceptionHandler<in TException> where TException : Exception
    {
        Task Handle(TException exception);
    }
}

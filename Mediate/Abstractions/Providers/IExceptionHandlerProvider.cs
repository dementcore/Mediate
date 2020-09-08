using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Abstractions
{
    public interface IExceptionHandlerProvider
    {
        Task<IExceptionHandler<TException>> GetHandler<TException>(TException exception) where TException : Exception;
    }
}

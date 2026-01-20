using System;

namespace Projeto.Core.Infrastructure.Exceptions
{
    /// <summary>
    /// Classe base para exceções de negócio.
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}

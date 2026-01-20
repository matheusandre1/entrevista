using System.Data;

namespace Projeto.Core.Infrastructure.Database
{
    /// <summary>
    /// @ref https://refactoring.guru/design-patterns/factory-method
    /// </summary>
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

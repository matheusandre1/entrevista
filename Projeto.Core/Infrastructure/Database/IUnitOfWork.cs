using System;
using System.Data;

/// <summary>
/// Utilizando um Padrão Unit of Work para gerenciar transações de banco de dados.
/// @ref: https://martinfowler.com/eaaCatalog/unitOfWork.html
/// </summary>
namespace Projeto.Core.Infrastructure.Database
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Begin();
        void Commit();
        void Rollback();
    }
}

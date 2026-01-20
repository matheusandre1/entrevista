using Dapper;
using System.Collections.Generic;

namespace Projeto.Core.Infrastructure.Database
{
    public abstract class Repository<T> : IRepository<T> where T : IEntity
    {
        protected readonly IUnitOfWork _uow;
        protected readonly string _tableName;

        protected Repository(IUnitOfWork uow, string tableName)
        {
            _uow = uow;
            _tableName = tableName;
        }

        public virtual T GetById(int id)
        {
            return _uow.Connection.QuerySingleOrDefault<T>($"SELECT * FROM {_tableName} WHERE id = @Id", new { Id = id }, _uow.Transaction);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _uow.Connection.Query<T>($"SELECT * FROM {_tableName}", transaction: _uow.Transaction);
        }

        public abstract void Insert(T entity);
        public abstract void Update(T entity);

        public virtual void Delete(int id)
        {
            _uow.Connection.Execute($"DELETE FROM {_tableName} WHERE id = @Id", new { Id = id }, _uow.Transaction);
        }
    }
}

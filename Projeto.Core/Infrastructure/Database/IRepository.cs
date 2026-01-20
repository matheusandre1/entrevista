using System.Collections.Generic;

namespace Projeto.Core.Infrastructure.Database
{
    public interface IRepository<T> where T : IEntity
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}

using System.Collections.Generic;

namespace Projeto.Core.Infrastructure.Database
{
    /// <summary>
    /// Interface genérica para serviços de CRUD. - Como Repository Pattern - 
    /// @ref: https://dotnettutorials.net/lesson/repository-design-pattern-csharp/
    /// </summary>
    /// <typeparam name="T">Tipo da entidade que implementa IEntity.</typeparam>
    public interface ICrudService<T> 
        where T: IEntity
    {        
        public T GetById(int id);
        public IEnumerable<T> GetAll();
        public void Insert(T entity);        
        public void Update(T entity);
        public void Delete(T entity);
    }
}

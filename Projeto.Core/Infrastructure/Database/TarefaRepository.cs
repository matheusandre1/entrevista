using Dapper;
using Projeto.Core.Entity;

namespace Projeto.Core.Infrastructure.Database
{
    public interface ITarefaRepository : IRepository<Tarefa>
    {
    }

    public class TarefaRepository : Repository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(IUnitOfWork uow) : base(uow, "tarefa")
        {
        }

        public override void Insert(Tarefa entity)
        {
            const string sql = "INSERT INTO tarefa (tipo, descricao, concluida) VALUES (@Tipo, @Descricao, @Concluida); SELECT last_insert_rowid();";
            entity.Id = _uow.Connection.ExecuteScalar<int>(sql, entity, _uow.Transaction);
        }

        public override void Update(Tarefa entity)
        {
            const string sql = "UPDATE tarefa SET tipo = @Tipo, descricao = @Descricao, concluida = @Concluida WHERE id = @Id";
            _uow.Connection.Execute(sql, entity, _uow.Transaction);
        }
    }
}

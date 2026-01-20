using Dapper;
using Projeto.Core.Entity;

namespace Projeto.Core.Infrastructure.Database
{
    public interface ILogRepository : IRepository<Log>
    {
    }

    public class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(IUnitOfWork uow) : base(uow, "log")
        {
        }

        public override void Insert(Log entity)
        {
            const string sql = "INSERT INTO log (data, mensagem, usuario) VALUES (@Data, @Mensagem, @Usuario); SELECT last_insert_rowid();";
            entity.Id = _uow.Connection.ExecuteScalar<int>(sql, entity, _uow.Transaction);
        }

        public override void Update(Log entity)
        {
            const string sql = "UPDATE log SET data = @Data, mensagem = @Mensagem, usuario = @Usuario WHERE id = @Id";
            _uow.Connection.Execute(sql, entity, _uow.Transaction);
        }
    }
}

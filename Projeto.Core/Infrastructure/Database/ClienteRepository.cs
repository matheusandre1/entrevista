using Dapper;
using Projeto.Core.Entity;

namespace Projeto.Core.Infrastructure.Database
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Cliente GetByEmail(string email);
    }

    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(IUnitOfWork uow) : base(uow, "cliente")
        {
        }

        public Cliente GetByEmail(string email)
        {
            return _uow.Connection.QuerySingleOrDefault<Cliente>(
                $"SELECT id, nome, data_nascimento as DataNascimento, email, telefone, cidade, genero, usuario_responsavel_id as UsuarioResponsavelId FROM {_tableName} WHERE email = @Email", 
                new { Email = email }, 
                _uow.Transaction);
        }

        public override Cliente GetById(int id)
        {
            return _uow.Connection.QuerySingleOrDefault<Cliente>(
                $"SELECT id, nome, data_nascimento as DataNascimento, email, telefone, cidade, genero, usuario_responsavel_id as UsuarioResponsavelId FROM {_tableName} WHERE id = @Id", 
                new { Id = id }, 
                _uow.Transaction);
        }

        public override System.Collections.Generic.IEnumerable<Cliente> GetAll()
        {
            return _uow.Connection.Query<Cliente>(
                $"SELECT id, nome, data_nascimento as DataNascimento, email, telefone, cidade, genero, usuario_responsavel_id as UsuarioResponsavelId FROM {_tableName}", 
                transaction: _uow.Transaction);
        }

        public override void Insert(Cliente entity)
        {
            const string sql = "INSERT INTO cliente (nome, data_nascimento, email, telefone, cidade, genero, usuario_responsavel_id) VALUES (@Nome, @DataNascimento, @Email, @Telefone, @Cidade, @Genero, @UsuarioResponsavelId); SELECT last_insert_rowid();";
            entity.Id = _uow.Connection.ExecuteScalar<int>(sql, entity, _uow.Transaction);
        }

        public override void Update(Cliente entity)
        {
            const string sql = "UPDATE cliente SET nome = @Nome, data_nascimento = @DataNascimento, email = @Email, telefone = @Telefone, cidade = @Cidade, genero = @Genero, usuario_responsavel_id = @UsuarioResponsavelId WHERE id = @Id";
            _uow.Connection.Execute(sql, entity, _uow.Transaction);
        }
    }
}

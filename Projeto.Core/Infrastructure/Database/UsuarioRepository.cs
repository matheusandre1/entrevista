using Dapper;
using Projeto.Core.Entity;
using System.Linq;

namespace Projeto.Core.Infrastructure.Database
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario GetByLogin(string login);
    }

    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IUnitOfWork uow) : base(uow, "usuario")
        {
        }

        public Usuario GetByLogin(string login)
        {
            return _uow.Connection.QuerySingleOrDefault<Usuario>(
                $"SELECT id, nome, login, senha, is_admin as IsAdmin, is_ativo as IsAtivo FROM {_tableName} WHERE login = @Login", 
                new { Login = login }, 
                _uow.Transaction);
        }

        public override void Insert(Usuario entity)
        {
            const string sql = "INSERT INTO usuario (nome, login, senha, is_admin, is_ativo) VALUES (@Nome, @Login, @Senha, @IsAdmin, @IsAtivo); SELECT last_insert_rowid();";
            entity.Id = _uow.Connection.ExecuteScalar<int>(sql, entity, _uow.Transaction);
        }

        public override void Update(Usuario entity)
        {
            const string sql = "UPDATE usuario SET nome = @Nome, login = @Login, senha = @Senha, is_admin = @IsAdmin, is_ativo = @IsAtivo WHERE id = @Id";
            _uow.Connection.Execute(sql, entity, _uow.Transaction);
        }
    }
}

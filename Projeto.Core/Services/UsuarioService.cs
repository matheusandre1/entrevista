using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using Projeto.Core.Infrastructure.Exceptions;
using System.Collections.Generic;

namespace Projeto.Core.Services
{
    public interface IUsuarioService : ICrudService<Usuario>
    {
        Usuario Authenticate(string login, string senha);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _uow;

        public UsuarioService(IUsuarioRepository usuarioRepository, IUnitOfWork uow)
        {
            _usuarioRepository = usuarioRepository;
            _uow = uow;
        }

        public void Delete(Usuario entity)
        {
            _usuarioRepository.Delete(entity.Id);
        }

        public Usuario GetById(int id)
        {
            return _usuarioRepository.GetById(id);
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _usuarioRepository.GetAll();
        }

        public void Insert(Usuario entity)
        {
            var existing = _usuarioRepository.GetByLogin(entity.Login);
            if (existing != null)
            {
                throw new BusinessException("Login já cadastrado");
            }

            _usuarioRepository.Insert(entity);
        }

        public void Update(Usuario entity)
        {
            _usuarioRepository.Update(entity);
        }

        public Usuario Authenticate(string login, string senha)
        {
            var user = _usuarioRepository.GetByLogin(login);
            if (user != null && user.Senha == senha && user.IsAtivo)
            {
                return user;
            }
            return null;
        }
    }
}

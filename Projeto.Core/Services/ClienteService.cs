using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using Projeto.Core.Infrastructure.Exceptions;

namespace Projeto.Core.Services
{
    public class ClienteService : ICrudService<Cliente>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUnitOfWork _uow;
        private readonly ILogService _logService;

        public ClienteService(IClienteRepository clienteRepository, IUnitOfWork uow, ILogService logService)
        {
            _clienteRepository = clienteRepository;
            _uow = uow;
            _logService = logService;
        }

        private void Validate(Cliente entity)
        {
            var existingCliente = _clienteRepository.GetByEmail(entity.Email);
            if (existingCliente != null && existingCliente.Id != entity.Id)
            {
                throw new BusinessException("O e-mail informado já está cadastrado");
            }
        }

        public void Delete(Cliente entity)
        {
            _logService.LogChange($"Cliente excluído: ID={entity.Id}, Nome={entity.Nome}");
            _clienteRepository.Delete(entity.Id);
        }

        public Cliente GetById(int id)
        {
            return _clienteRepository.GetById(id);
        }

        public System.Collections.Generic.IEnumerable<Cliente> GetAll()
        {
            return _clienteRepository.GetAll();
        }

        public void Insert(Cliente entity)
        {
            Validate(entity);
            _clienteRepository.Insert(entity);
            _logService.LogChange($"Novo cliente inserido: ID={entity.Id}, Nome={entity.Nome}");
        }

        public void Update(Cliente entity)
        {
            Validate(entity);
            _clienteRepository.Update(entity);
            _logService.LogChange($"Cliente atualizado: ID={entity.Id}, Nome={entity.Nome}");
        }
    }
}

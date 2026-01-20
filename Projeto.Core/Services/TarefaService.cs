using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;

namespace Projeto.Core.Services
{
    public class TarefaService : ICrudService<Tarefa>
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IUnitOfWork _uow;

        public TarefaService(ITarefaRepository tarefaRepository, IUnitOfWork uow)
        {
            _tarefaRepository = tarefaRepository;
            _uow = uow;
        }

        public void Delete(Tarefa entity)
        {
            _tarefaRepository.Delete(entity.Id);
        }

        public Tarefa GetById(int id)
        {
            return _tarefaRepository.GetById(id);
        }

        public System.Collections.Generic.IEnumerable<Tarefa> GetAll()
        {
            return _tarefaRepository.GetAll();
        }

        public void Insert(Tarefa entity)
        {
            _tarefaRepository.Insert(entity);
        }

        public void Update(Tarefa entity)
        {
            _tarefaRepository.Update(entity);
        }
    }
}

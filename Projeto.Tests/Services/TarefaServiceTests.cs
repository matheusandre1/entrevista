using AutoFixture;
using Moq;
using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using Projeto.Core.Services;
using Xunit;

namespace Projeto.Tests.Services
{
    public class TarefaServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly TarefaService _tarefaService;

        public TarefaServiceTests()
        {
            _fixture = new Fixture();
            _tarefaRepositoryMock = new Mock<ITarefaRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _tarefaService = new TarefaService(_tarefaRepositoryMock.Object, _uowMock.Object);
        }

        [Fact]
        public void Insert_ShouldCallRepository()        {
            
            var tarefa = _fixture.Create<Tarefa>();
            
            _tarefaService.Insert(tarefa);
            
            _tarefaRepositoryMock.Verify(r => r.Insert(tarefa), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallRepository()
        {
            
            var tarefa = _fixture.Create<Tarefa>();

            
            _tarefaService.Update(tarefa);
            
            _tarefaRepositoryMock.Verify(r => r.Update(tarefa), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallRepository()
        {            
            var tarefa = _fixture.Create<Tarefa>();

            
            _tarefaService.Delete(tarefa);
            
            _tarefaRepositoryMock.Verify(r => r.Delete(tarefa.Id), Times.Once);
        }

        [Fact]
        public void GetById_ShouldReturnFromRepository()
        {            
            var tarefa = _fixture.Create<Tarefa>();
            _tarefaRepositoryMock.Setup(r => r.GetById(tarefa.Id)).Returns(tarefa);

            
            var result = _tarefaService.GetById(tarefa.Id);
            
            Assert.Equal(tarefa, result);
            _tarefaRepositoryMock.Verify(r => r.GetById(tarefa.Id), Times.Once);
        }
    }
}

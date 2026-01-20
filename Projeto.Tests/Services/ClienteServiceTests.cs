using AutoFixture;
using Moq;
using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using Projeto.Core.Infrastructure.Exceptions;
using Projeto.Core.Services;
using System;
using Xunit;

namespace Projeto.Tests.Services
{
    public class ClienteServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly ClienteService _clienteService;
        private readonly ILogService _logService;

        public ClienteServiceTests()
        {
            _fixture = new Fixture();
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _clienteService = new ClienteService(_clienteRepositoryMock.Object, _uowMock.Object, _logService);
        }

        [Fact]
        public void Insert_ShouldThrowException_WhenEmailAlreadyExists()
        {            
            var existingCliente = _fixture.Create<Cliente>();
            var newCliente = _fixture.Build<Cliente>()
                .With(c => c.Email, existingCliente.Email)
                .With(c => c.Id, existingCliente.Id + 1)
                .Create();

            _clienteRepositoryMock.Setup(r => r.GetByEmail(newCliente.Email))
                .Returns(existingCliente);

            
            var exception = Assert.Throws<BusinessException>(() => _clienteService.Insert(newCliente));
            Assert.Equal("O e-mail informado já está cadastrado", exception.Message);
            _clienteRepositoryMock.Verify(r => r.Insert(It.IsAny<Cliente>()), Times.Never);
        }

        [Fact]
        public void Insert_ShouldCallRepository_WhenEmailDoesNotExist()
        {
            
            var newCliente = _fixture.Create<Cliente>();

            _clienteRepositoryMock.Setup(r => r.GetByEmail(newCliente.Email))
                .Returns((Cliente)null);

            
            _clienteService.Insert(newCliente);
            
            _clienteRepositoryMock.Verify(r => r.Insert(newCliente), Times.Once);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenEmailAlreadyExistsForAnotherId()
        {
            var existingCliente = _fixture.Create<Cliente>();
            var clienteToUpdate = _fixture.Build<Cliente>()
                .With(c => c.Email, existingCliente.Email)
                .With(c => c.Id, existingCliente.Id + 1)
                .Create();

            _clienteRepositoryMock.Setup(r => r.GetByEmail(clienteToUpdate.Email))
                .Returns(existingCliente);
           
            var exception = Assert.Throws<BusinessException>(() => _clienteService.Update(clienteToUpdate));
            Assert.Equal("O e-mail informado já está cadastrado", exception.Message);
            _clienteRepositoryMock.Verify(r => r.Update(It.IsAny<Cliente>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldCallRepository_WhenEmailIsSameForSameId()
        {
            
            var existingCliente = _fixture.Create<Cliente>();
            
            _clienteRepositoryMock.Setup(r => r.GetByEmail(existingCliente.Email))
                .Returns(existingCliente);
            
            _clienteService.Update(existingCliente);
            
            _clienteRepositoryMock.Verify(r => r.Update(existingCliente), Times.Once);
        }
    }
}

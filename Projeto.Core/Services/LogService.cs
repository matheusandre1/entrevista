using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using System;

namespace Projeto.Core.Services
{
    public interface ILogService
    {
        void LogChange(string mensagem, string usuario = "Sistema");
    }

    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _uow;

        public LogService(ILogRepository logRepository, IUnitOfWork uow)
        {
            _logRepository = logRepository;
            _uow = uow;
        }

        public void LogChange(string mensagem, string usuario = "Sistema")
        {
            var log = new Log
            {
                Data = DateTime.Now,
                Mensagem = mensagem,
                Usuario = usuario
            };

            _logRepository.Insert(log);
        }
    }
}

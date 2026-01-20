using Projeto.Core.Infrastructure.Database;
using System;

namespace Projeto.Core.Entity
{
    public class Log : IEntity
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Mensagem { get; set; }
        public string Usuario { get; set; }
    }
}

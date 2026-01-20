using Projeto.Core.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Core.Entity
{
    public class Tarefa : IEntity
    {
        public Tarefa() { }

        public Tarefa(string tipo, string descricao)
        {
            this.Tipo = tipo;
            this.Descricao = descricao;
        }

        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public bool Concluida { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Core.Infrastructure.Database
{
    /// <summary>
    /// Interface base para todas as entidades do sistema.
    /// </summary>
    public interface IEntity
    {        
        public int Id { get; set; }
    }
}

using Projeto.Core.Infrastructure.Database;
using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto.Core.Entity
{
    public class Usuario : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Administrador")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Ativo")]
        public bool IsAtivo { get; set; }
    }
}

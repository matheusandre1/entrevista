using Projeto.Core.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Projeto.Core.Entity
{
    public class Cliente : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(100, ErrorMessage = "O limite máximo de caracteres para o campo foi atingido.")]
        [Display(Name= "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Data Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(120, ErrorMessage = "O limite máximo de caracteres para o campo foi atingido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Display(Name = "Gênero")]
        public string Genero { get; set; }

        [Display(Name = "Responsável")]
        public int? UsuarioResponsavelId { get; set; }
    }
}

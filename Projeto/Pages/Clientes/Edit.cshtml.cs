using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using Projeto.Core.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Linq;

namespace Projeto.Pages.Clientes
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ICrudService<Cliente> _clienteService;
        private readonly IUsuarioService _usuarioService;

        public EditModel(ICrudService<Cliente> clienteService, IUsuarioService usuarioService)
        {
            _clienteService = clienteService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        public IEnumerable<Usuario> Usuarios { get; set; }

        public string Erro { get; set; }

        public void OnGet(int id)
        {
            this.Usuarios = _usuarioService.GetAll();
            this.Cliente = _clienteService.GetById(id);
        }

        public IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.Usuarios = _usuarioService.GetAll();
                    return Page();
                }

                _clienteService.Update(this.Cliente);

                TempData["MensagemSucesso"] = "Registro atualizado com sucesso.";

                return RedirectToPage("./Index");
            }
            catch (System.Exception ex)
            {
                this.Erro = ex.Message;
            }

            return Page();
        }
    }
}

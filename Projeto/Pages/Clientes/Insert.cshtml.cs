using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using Projeto.Core.Services;
using System;

namespace Projeto.Pages.Clientes
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class InsertModel : PageModel
    {
        private readonly ICrudService<Cliente> _clienteService;
        private readonly IUsuarioService _usuarioService;

        public InsertModel(ICrudService<Cliente> clienteService, IUsuarioService usuarioService)
        {
            _clienteService = clienteService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        public System.Collections.Generic.IEnumerable<Usuario> Usuarios { get; set; }

        public string Erro { get; set; }

        public void OnGet()
        {
            this.Usuarios = _usuarioService.GetAll();
            this.Cliente = new Cliente() { DataNascimento = System.DateTime.Now };
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

                _clienteService.Insert(this.Cliente);

                TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso.";

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                this.Erro = ex.Message;
                return Page();
            }
        }
    }
}

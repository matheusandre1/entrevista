using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using Projeto.Core.Services;
using System;

namespace Projeto.Pages.Clientes
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ICrudService<Cliente> _clienteService;

        public DeleteModel(ICrudService<Cliente> clienteService)
        {
            _clienteService = clienteService;
        }

        [BindProperty]
        public Cliente Cliente { get; set; }

        public string Erro { get; set; }

        public void OnGet(int id)
        {
            this.Cliente = _clienteService.GetById(id);
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                var cliente = _clienteService.GetById(id);
                _clienteService.Delete(cliente);

                TempData["MensagemSucesso"] = "Registro excluído com sucesso.";

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

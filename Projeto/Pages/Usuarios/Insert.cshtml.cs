using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto.Core.Entity;
using Projeto.Core.Services;
using System;

namespace Projeto.Pages.Usuarios
{
    [Authorize(Roles = "Admin")]
    public class InsertModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public InsertModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public Usuario Usuario { get; set; }

        public string Erro { get; set; }

        public void OnGet()
        {
            this.Usuario = new Usuario { IsAtivo = true };
        }

        public IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                _usuarioService.Insert(this.Usuario);

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

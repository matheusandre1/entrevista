using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto.Core.Entity;
using Projeto.Core.Services;
using System.Collections.Generic;

namespace Projeto.Pages.Usuarios
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public IndexModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IEnumerable<Usuario> Usuarios { get; set; }

        public void OnGet()
        {
            this.Usuarios = _usuarioService.GetAll();
        }
    }
}

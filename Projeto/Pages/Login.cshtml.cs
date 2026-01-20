using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto.Core.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projeto.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public LoginModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public string Mensagem { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Senha))
            {
                Mensagem = "Por favor, preencha o login e a senha.";
                return Page();
            }

            var usuario = _usuarioService.Authenticate(Login, Senha);

            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Role, usuario.IsAdmin ? "Admin" : "User"),
                    new Claim("UserId", usuario.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Index");
            }

            Mensagem = "Login ou senha inválidos.";
            return Page();
        }
    }
}

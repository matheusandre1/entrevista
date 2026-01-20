using Microsoft.AspNetCore.Mvc.RazorPages;
using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using System.Collections.Generic;
using System.Linq;

namespace Projeto.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        private readonly ICrudService<Cliente> _clienteService;

        public IndexModel(ICrudService<Cliente> clienteService)
        {
            _clienteService = clienteService;
        }

        public List<Cliente> Clientes { get; set; }

        public void OnGet()
        {
            this.Clientes = _clienteService.GetAll().ToList();
        }
    }
}

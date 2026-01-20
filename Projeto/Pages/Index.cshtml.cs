using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Projeto.Core.Entity;
using Projeto.Core.Infrastructure.Database;
using Projeto.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Pages
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICrudService<Tarefa> _crudService;

        private const string DBNAME = "banco.db";

        [BindProperty]
        public List<Tarefa> Tarefas { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IHostingEnvironment hostingEnvironment,
            ICrudService<Tarefa> crudService)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _crudService = crudService;
        }

        public void OnGet()
        {
            /* 
             * A aplicação controla as migrações através da tabela 'migration'.
             * Cada bloco de alteração no banco de dados (CREATE TABLE, ALTER TABLE, SEED) 
             * possui um identificador único (ex: 'm1', 'm2', 'seed1').
             * Antes de executar uma instrução, o sistema verifica se o identificador já existe na tabela 'migration'.
             * Se não existir, a instrução é executada e o identificador é registrado, garantindo que 
             * a mesma alteração nunca seja aplicada mais de uma vez.
             * 
             * Nota: Este código foi refatorado para o DatabaseInitializer.cs, mas mantemos o comentário 
             * aqui conforme solicitado no TODO original para documentação.
             */

            this.Tarefas = _crudService.GetAll().ToList();
        }

        public ActionResult OnPost()
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            foreach (var tarefa in this.Tarefas)
            {
                _crudService.Update(tarefa);
            }

            return Page();
        }
    }
}

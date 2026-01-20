using Dapper;
using Projeto.Core.Entity;
using System;
using System.Data.SQLite;
using System.Linq;

namespace Projeto.Core.Infrastructure.Database
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ICrudService<Tarefa> _tarefaService;
        private const string DBNAME = "banco.db";

        public DatabaseInitializer(
            IConnectionFactory connectionFactory,
            ICrudService<Tarefa> tarefaService)
        {
            _connectionFactory = connectionFactory;
            _tarefaService = tarefaService;
        }

        /// <summary>
        /// Inicializa o banco de dados executando todas as migrations pendentes.
        /// 
        /// Sistema de Controle de Migrations:
        /// - A tabela 'migration' armazena códigos únicos de cada migration executada
        /// - Antes de executar uma migration, verifica-se se seu código já existe na tabela
        /// - Se o código não existe, a migration é executada e o código é inserido
        /// - Isso garante que cada migration seja executada apenas uma vez, mesmo que a aplicação
        ///   seja reiniciada múltiplas vezes
        /// </summary>
        public void Initialize()
        {
            // Cria o arquivo do banco de dados se não existir
            if (!System.IO.File.Exists(DBNAME))
            {
                SQLiteConnection.CreateFile(DBNAME);
            }

            using var connection = (SQLiteConnection)_connectionFactory.CreateConnection();
            connection.Open();

            // Cria a tabela de controle de migrations se não existir
            CreateMigrationTable(connection);

            // Busca todas as migrations já executadas
            var executedMigrations = connection.Query<string>("SELECT code FROM migration").ToList();

            // Executa migrations pendentes
            ExecuteMigrations(connection, executedMigrations);
        }

        /// <summary>
        /// Cria a tabela de controle de migrations se ela não existir.
        /// Esta tabela armazena os códigos das migrations já executadas.
        /// </summary>
        private void CreateMigrationTable(SQLiteConnection connection)
        {
            var migrationTable = connection.Query<string>(
                "SELECT name FROM sqlite_master WHERE type='table' AND name = 'migration'"
            ).FirstOrDefault();

            if (string.IsNullOrEmpty(migrationTable))
            {
                connection.Execute("CREATE TABLE migration (code TEXT PRIMARY KEY)");
            }
        }

        /// <summary>
        /// Executa todas as migrations pendentes (aquelas que ainda não foram executadas).
        /// Cada migration é identificada por um código único e só é executada se esse código
        /// não estiver presente na tabela 'migration'.
        /// </summary>
        private void ExecuteMigrations(SQLiteConnection connection, System.Collections.Generic.List<string> executedMigrations)
        {
            // Migration m1: Cria tabela de clientes
            if (!executedMigrations.Contains("m1"))
            {
                connection.Execute(
                    @"CREATE TABLE cliente (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       nome             TEXT        NOT NULL,
                       data_nascimento  DATETIME    NOT NULL,
                       email            TEXT,
                       telefone         TEXT,
                       cidade           TEXT,
                       genero           TEXT,
                       usuario_responsavel_id INTEGER,
                       FOREIGN KEY(usuario_responsavel_id) REFERENCES usuario(id)
                    )");

                connection.Execute("INSERT INTO migration VALUES('m1')");
            }

            // Migration m2: Removida pois email já foi incluído no m1 refatorado
            if (!executedMigrations.Contains("m2"))
            {
                // Já incluído no m1
                connection.Execute("INSERT INTO migration VALUES('m2')");
            }

            // Seed1: Popula tabela de clientes com dados iniciais
            if (!executedMigrations.Contains("seed1"))
            {
                string sql = "INSERT INTO cliente (nome, data_nascimento, email, telefone, cidade, genero, usuario_responsavel_id) Values (@Nome, @DataNascimento, @Email, @Telefone, @Cidade, @Genero, @UsuarioResponsavelId);";

                connection.Execute(sql,
                    new[]
                    {
                        new Cliente() { Nome = "Joaquim Ferreira", Email = "joaquim.ferreira@summit.com.br", DataNascimento = new DateTime(1990, 01, 17), Telefone = "11999998888", Cidade = "São Paulo", Genero = "Masculino", UsuarioResponsavelId = 1 },
                        new Cliente() { Nome = "Maria Fernandes", Email = "maria.fernandes@summit.com.br", DataNascimento = new DateTime(1993, 03, 21), Telefone = "11988887777", Cidade = "Rio de Janeiro", Genero = "Feminino", UsuarioResponsavelId = 1 },
                    });

                connection.Execute("INSERT INTO migration VALUES('seed1')");
            }

            // Migration tarefa: Cria tabela de tarefas e popula com dados iniciais
            if (!executedMigrations.Contains("tarefa"))
            {
                connection.Execute(
                    @"CREATE TABLE tarefa (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       tipo       TEXT       NOT NULL,
                       descricao  TEXT       NOT NULL,
                       concluida  BOOLEAN    NOT NULL
                    )");

                // Popula tarefas usando o serviço
                _tarefaService.Insert(new Tarefa("💄style", "O formulário de edição de clientes está sendo exibido na direita da tela. Coloque-o logo abaixo do título;"));
                _tarefaService.Insert(new Tarefa("💄style", "No formulário de exclusão de clientes, altere a cor do botão de azul para vermelho;"));
                _tarefaService.Insert(new Tarefa("🐛bug", "Na edição do cadastro de clientes, o campo e-mail está com uma restrição para aceitar apenas 12 caracteres, aumente para 120;"));
                _tarefaService.Insert(new Tarefa("🐛bug", "Na listagem do cadastro de clientes, a edição está apresentando um comportamento estranho, os dados apresentados nem sempre representam o registro selecionado. Corrija esse problema;"));
                _tarefaService.Insert(new Tarefa("🐛bug", "Na edição do cadastro de clientes, o campo data de nascimento não está sendo atualizado. Corrija este problema;"));
                _tarefaService.Insert(new Tarefa("✨feature", "O cadastro de clientes está incompleto, inclua os campos telefone, cidade e gênero;"));
                _tarefaService.Insert(new Tarefa("✨feature", "Inclua as colunas faltantes na exibição da listagem de clientes (data de nascimento, telefone, cidade e gênero);"));
                _tarefaService.Insert(new Tarefa("♻️refactor", "As migrations (controle de criação e atualização das tabelas do banco de dados) estão em um local inapropriado no código fonte. Faça um melhor gerenciamento para isso;"));
                _tarefaService.Insert(new Tarefa("♻️refactor", "Encapsule todo código fonte inerente a regras de negócio dentro de classes de serviço (reaproveite o máximo de código que puder);"));
                _tarefaService.Insert(new Tarefa("♻️refactor", "A conexão com o banco de dados em toda a aplicação está sendo feita de forma redundante. Faça um melhor gerenciamento para isso;"));
                _tarefaService.Insert(new Tarefa("♻️refactor", "Aplique injeção de dependência para fornecer os serviços para a aplicação;"));
                _tarefaService.Insert(new Tarefa("✨feature", "Inclua a funcionalidade para cadastrar um novo cliente;"));
                _tarefaService.Insert(new Tarefa("💡comments", "Existem TODOs (tarefas pendentes) espalhadas pelo código fonte (bem como adicionar comentários em classes ou tratar algum erro). Localize esses itens e implemente a solução;"));
                _tarefaService.Insert(new Tarefa("✨feature", "Crie um sistema de log para a aplicação. Grave todas as vezes que um registro for alterado no sistema;"));
                _tarefaService.Insert(new Tarefa("✨feature", "Crie um cadastro de usuários para o sistema. O cadastro deve ter login, senha, nome, se é administrador ou não e se está ativo ou inativo. (não é necessário implementar edição ou exclusão de registros);"));
                _tarefaService.Insert(new Tarefa("✨feature", "Crie um campo de usuário responsável no cadastro de clientes, deve ser do tipo seleção e refletir os dados do cadastro de usuários;"));
                _tarefaService.Insert(new Tarefa("✨feature", "Implemente o login da aplicação, fazendo a autenticação dos usuários com base no cadastro de usuários;"));
                _tarefaService.Insert(new Tarefa("✨feature", "Garanta que apenas usuários administradores possam dar manutenção no cadastro de usuários;"));

                connection.Execute("INSERT INTO migration VALUES('tarefa')");
            }

            // Migration log: Cria tabela de logs
            if (!executedMigrations.Contains("log"))
            {
                connection.Execute(
                    @"CREATE TABLE log (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       data      DATETIME    NOT NULL,
                       mensagem  TEXT        NOT NULL,
                       usuario   TEXT        NOT NULL
                    )");

                connection.Execute("INSERT INTO migration VALUES('log')");
            }

            // Migration usuario: Cria tabela de usuários
            if (!executedMigrations.Contains("usuario"))
            {
                connection.Execute(
                    @"CREATE TABLE usuario (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       nome     TEXT        NOT NULL,
                       login    TEXT        NOT NULL UNIQUE,
                       senha    TEXT        NOT NULL,
                       is_admin BOOLEAN     NOT NULL,
                       is_ativo BOOLEAN     NOT NULL
                    )");

                // Seed de usuários iniciais
                connection.Execute(
                    "INSERT INTO usuario (nome, login, senha, is_admin, is_ativo) VALUES (@Nome, @Login, @Senha, @IsAdmin, @IsAtivo)",
                    new[]
                    {
                        new Usuario { Nome = "Administrador", Login = "admin", Senha = "123", IsAdmin = true, IsAtivo = true },
                        new Usuario { Nome = "Usuário Comum", Login = "user", Senha = "123", IsAdmin = false, IsAtivo = true }
                    });

                connection.Execute("INSERT INTO migration VALUES('usuario')");
            }

            // Migration m3: Adiciona colunas faltantes na tabela cliente se elas não existirem
            if (!executedMigrations.Contains("m3"))
            {
                var tableInfo = connection.Query<dynamic>("PRAGMA table_info('cliente')").ToList();
                var columns = tableInfo.Select(c => (string)c.name).ToList();

                if (!columns.Contains("telefone"))
                    connection.Execute("ALTER TABLE cliente ADD COLUMN telefone TEXT");

                if (!columns.Contains("cidade"))
                    connection.Execute("ALTER TABLE cliente ADD COLUMN cidade TEXT");

                if (!columns.Contains("genero"))
                    connection.Execute("ALTER TABLE cliente ADD COLUMN genero TEXT");

                if (!columns.Contains("usuario_responsavel_id"))
                    connection.Execute("ALTER TABLE cliente ADD COLUMN usuario_responsavel_id INTEGER");

                connection.Execute("INSERT INTO migration VALUES('m3')");
            }
        }
    }
}

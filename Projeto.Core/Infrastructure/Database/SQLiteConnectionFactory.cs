using System.Data;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration;

namespace Projeto.Core.Infrastructure.Database
{
    public class SQLiteConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public SQLiteConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }
}

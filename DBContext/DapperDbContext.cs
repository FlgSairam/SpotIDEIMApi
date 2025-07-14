using System.Data;
using MySql.Data.MySqlClient;
namespace DapperAuthApi.DBContext
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connection;

        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection() => new MySqlConnection(_connection);

    }
}

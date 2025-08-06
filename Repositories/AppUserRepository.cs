using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using DapperAuthApi.Models;

namespace DapperAuthApi.Repositories
{
    public class AppUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public AppUserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            using var db = Connection;
            string sql = "SELECT * FROM users WHERE record_status = 1";
            return await db.QueryAsync<AppUser>(sql);
        }

        public async Task<AppUser?> GetByIdAsync(long id)
        {
            using var db = Connection;
            string sql = "SELECT * FROM users WHERE pid = @Id";
            return await db.QueryFirstOrDefaultAsync<AppUser>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(AppUser user)
        {
            using var db = Connection;
            string sql = @"
                INSERT INTO users 
                (username, password, role, email, mobile, is_active, record_status, created_by, created_date)
                VALUES
                (@Username, @Password, @Role, @Email, @Mobile, @Is_Active, 1, @Created_By, CURRENT_TIMESTAMP)";
            return await db.ExecuteAsync(sql, user);
        }

        public async Task<int> UpdateAsync(AppUser user)
        {
            using var db = Connection;
            string sql = @"
                UPDATE users SET 
                    username = @Username,
                    password = @Password,
                    role = @Role,
                    email = @Email,
                    mobile = @Mobile,
                    is_active = @Is_Active,
                    updated_by = @Updated_By,
                    updated_date = CURRENT_TIMESTAMP
                WHERE pid = @Pid";
            return await db.ExecuteAsync(sql, user);
        }

        public async Task<int> DeleteAsync(long id)
        {
            using var db = Connection;
            string sql = "UPDATE users SET record_status = 0 WHERE pid = @Id";
            return await db.ExecuteAsync(sql, new { Id = id });
        }
    }
}

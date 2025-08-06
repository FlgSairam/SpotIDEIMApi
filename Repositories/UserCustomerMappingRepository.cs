using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using DapperAuthApi.Models;

namespace DapperAuthApi.Repositories
{
    public class UserCustomerMappingRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public UserCustomerMappingRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<Customer>> GetMappedCustomersAsync(long userId)
        {
            using var db = Connection;
            string sql = @"
                SELECT c.*
                FROM eim_UserCustomerMapping m
                JOIN eim_customermasters c ON m.CustomerId = c.pid
                WHERE m.record_status = 1 AND m.UserId = @UserId";

            return await db.QueryAsync<Customer>(sql, new { UserId = userId });
        }

        public async Task SaveUserCustomerMappingAsync(long userId, List<long> customerIds, string? modifiedBy)
        {
            using var db = Connection;
            using var transaction = db.BeginTransaction();

            try
            {
                string softDelete = @"
                    UPDATE eim_UserCustomerMapping
                    SET record_status = 0, updated_by = @ModifiedBy, updated_date = NOW()
                    WHERE UserId = @UserId AND record_status = 1";

                await db.ExecuteAsync(softDelete, new { UserId = userId, ModifiedBy = modifiedBy }, transaction);

                string insertSql = @"
                    INSERT INTO eim_UserCustomerMapping (UserId, CustomerId, created_by)
                    VALUES (@UserId, @CustomerId, @CreatedBy)";

                foreach (var customerId in customerIds)
                {
                    await db.ExecuteAsync(insertSql, new
                    {
                        UserId = userId,
                        CustomerId = customerId,
                        CreatedBy = modifiedBy
                    }, transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

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

            try
            {
                // Step 1: Soft-delete all current mappings for the user
                string softDelete = @"
            UPDATE eim_UserCustomerMapping
            SET record_status = 0, updated_by = @ModifiedBy, updated_date = NOW()
            WHERE UserId = @UserId AND record_status = 1";

                await db.ExecuteAsync(softDelete, new { UserId = userId, ModifiedBy = modifiedBy });

                // Step 2: Loop through customer IDs
                foreach (var customerId in customerIds)
                {
                    // Step 2a: Check if mapping already exists
                    string checkSql = @"
                SELECT COUNT(*) FROM eim_UserCustomerMapping
                WHERE UserId = @UserId AND CustomerId = @CustomerId";

                    int count = await db.ExecuteScalarAsync<int>(checkSql, new
                    {
                        UserId = userId,
                        CustomerId = customerId
                    });

                    if (count > 0)
                    {
                        // Step 2b: Reactivate existing mapping
                        string reactivateSql = @"
                    UPDATE eim_UserCustomerMapping
                    SET record_status = 1, updated_by = @ModifiedBy, updated_date = NOW()
                    WHERE UserId = @UserId AND CustomerId = @CustomerId";

                        await db.ExecuteAsync(reactivateSql, new
                        {
                            UserId = userId,
                            CustomerId = customerId,
                            ModifiedBy = modifiedBy
                        });
                    }
                    else
                    {
                        // Step 2c: Insert new mapping
                        string insertSql = @"
                    INSERT INTO eim_UserCustomerMapping (UserId, CustomerId, created_by)
                    VALUES (@UserId, @CustomerId, @CreatedBy)";

                        await db.ExecuteAsync(insertSql, new
                        {
                            UserId = userId,
                            CustomerId = customerId,
                            CreatedBy = modifiedBy
                        });
                    }
                }
            }
            catch
            {
                throw;
            }
        }

    }
}

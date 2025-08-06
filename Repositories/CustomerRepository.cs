using Dapper;
using DapperAuthApi.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace DapperAuthApi.Repositories;

public class CustomerRepository
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public CustomerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        using var db = Connection;
        string sql = "SELECT * FROM eim_customermasters WHERE record_status = 1";
        return await db.QueryAsync<Customer>(sql);
    }

    public async Task<Customer?> GetByIdAsync(long id)
    {
        using var db = Connection;
        string sql = "SELECT * FROM eim_customermasters WHERE pid = @Id";
        return await db.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Customer customer)
    {
        using var db = Connection;
        string sql = @"
            INSERT INTO eim_customermasters 
            (consumer_name, short_name, consumer_address, contact_person_name, contact_no, email, website,      contract_period, created_by) 
            VALUES 
            (@Consumer_Name, @Short_Name, @Consumer_Address, @Contact_Person_Name, @Contact_No, @Email, @Website ,  @Contract_Period, @Created_By)";
        return await db.ExecuteAsync(sql, customer);
    }

    public async Task<int> UpdateAsync(long id, Customer customer)
    {
        using var db = Connection;
        string sql = @"
            UPDATE eim_customermasters 
            SET consumer_name = @Consumer_Name,
                short_name = @Short_Name,
                consumer_address = @Consumer_Address,
                contact_person_name = @Contact_Person_Name,
                contact_no = @Contact_No,
                email = @Email,
                website = @Website,
                contract_period = @Contract_Period,
                updated_by = @Updated_By,
                updated_date = CURRENT_TIMESTAMP
            WHERE pid = @Pid";
        customer.Pid = id;
        return await db.ExecuteAsync(sql, customer);
    }

    public async Task<int> DeleteAsync(long id)
    {
        using var db = Connection;
        string sql = "UPDATE eim_customermasters SET record_status = 0 WHERE pid = @Id";
        return await db.ExecuteAsync(sql, new { Id = id });
    }
}

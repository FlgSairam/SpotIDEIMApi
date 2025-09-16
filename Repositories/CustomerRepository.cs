using Dapper;
using DapperAuthApi.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace DapperAuthApi.Repositories;

/// <summary>
/// Repository for performing CRUD operations on Customer entities in the eim_customermasters table.
/// Uses Dapper for data access and MySQL as the database.
/// </summary>
public class CustomerRepository
{
    // Provides access to application configuration, including connection strings.
    private readonly IConfiguration _configuration;
    // Stores the database connection string.
    private readonly string? _connectionString;

    /// <summary>
    /// Initializes a new instance of the CustomerRepository class.
    /// </summary>
    /// <param name="configuration">Application configuration for retrieving connection string.</param>
    public CustomerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// Creates a new MySQL database connection using the configured connection string.
    /// </summary>
    private IDbConnection Connection => new MySqlConnection(_connectionString);

    /// <summary>
    /// Retrieves all active customers from the database.
    /// </summary>
    /// <returns>A list of Customer objects with record_status = 1.</returns>
    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        try
        {
            using var db = Connection;
            string sql = "SELECT * FROM eim_customermasters WHERE record_status = 1";
            return await db.QueryAsync<Customer>(sql);
        }
        catch (Exception)
        {
            // Log the exception (use your preferred logging framework)
            // _logger.LogError(ex, "Error retrieving customers.");
            throw new ApplicationException("An error occurred while retrieving customers.");
        }
    }

    /// <summary>
    /// Retrieves a customer by its primary key (pid).
    /// </summary>
    /// <param name="id">The primary key of the customer.</param>
    /// <returns>The Customer object if found; otherwise, null.</returns>
    public async Task<Customer?> GetByIdAsync(long id)
    {
        using var db = Connection;
        string sql = "SELECT * FROM eim_customermasters WHERE pid = @Id";
        return await db.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });
    }

    /// <summary>
    /// Inserts a new customer record into the database.
    /// </summary>
    /// <param name="customer">The Customer object to insert.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> CreateAsync(Customer customer)
    {
        using var db = Connection;
        string sql = @"
            INSERT INTO eim_customermasters 
            (consumer_name, short_name, consumer_address, contact_person_name, contact_no, email, website, contract_period, created_by) 
            VALUES 
            (@Consumer_Name, @Short_Name, @Consumer_Address, @Contact_Person_Name, @Contact_No, @Email, @Website, @Contract_Period, @Created_By)";
        return await db.ExecuteAsync(sql, customer);
    }

    /// <summary>
    /// Updates an existing customer record in the database.
    /// </summary>
    /// <param name="id">The primary key of the customer to update.</param>
    /// <param name="customer">The Customer object with updated values.</param>
    /// <returns>The number of rows affected.</returns>
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

    /// <summary>
    /// Soft-deletes a customer record by setting record_status to 0.
    /// </summary>
    /// <param name="id">The primary key of the customer to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> DeleteAsync(long id)
    {
        using var db = Connection;
        string sql = "UPDATE eim_customermasters SET record_status = 0 WHERE pid = @Id";
        return await db.ExecuteAsync(sql, new { Id = id });
    }
}

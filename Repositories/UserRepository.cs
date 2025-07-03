using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using DapperAuthApi.Models;

namespace DapperAuthApi.Repositories;

public class UserRepository
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);

    public async Task<User?> GetUserAsync(string username, string password)
    {
        using var db = Connection;
        string sql = "SELECT * FROM eim_usermasters WHERE  isactive=1 and username = @Username AND password = @Password";
        return await db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username, Password = password });
    }

    public async Task<MeterreaderInfo?> GetEmployeeInfoAsync(string accessId)
{
    using var db = Connection;
    string sql = @"
        SELECT 
            full_name AS name,
            photograph_url AS photograph,
            mobile_number AS contact_details, 
            discom,
            division,            
            reporting_officer_contact AS uppcl_reporting_officer_contact_details,
            service_status,
            agency_name
        FROM 
            eim_employee
        WHERE 
            record_status = 1 
            AND employee_access_id = @AccessId";

    var result = await db.QueryFirstOrDefaultAsync<MeterreaderInfo>(sql, new { AccessId = accessId });

    if (result == null)
    {
        return new MeterreaderInfo
        {
            Trx_Code = "01",
            Trx_Status = "Failure transaction no record found for this meter reader"
        };
    }

    if (result.Service_Status?.ToLower() == "inactive")
    {
        result.Trx_Code = "02";
        result.Trx_Status = "Inactive Meter Reader - No longer working with the agency Fluentgrid Limited";
    }
    else
    {
        result.Trx_Code = "00";
        result.Trx_Status = "Successful transaction";
    }

    return result;
}

}


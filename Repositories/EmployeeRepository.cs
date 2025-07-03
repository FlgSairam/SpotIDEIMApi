using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using DapperAuthApi.Models;

namespace DapperAuthApi.Repositories;

public class EmployeeRepository
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public EmployeeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);
 
 
    public async Task<EmployeeLoginInfo?> GetEmployeeLoginInfoAsync(string mobileNumber)
    {
        using var db = Connection;
        string sql = @"SELECT pid,  employee_id, offer_letter_no, CAST(employee_access_id AS CHAR) as employee_access_id, full_name, photograph_url, mobile_number, email, permanent_address, 
            date_of_joining, department, designation, father_name, blood_group, 
            work_location_fid, working_location, discom, discom_fid, division, division_fid,
            reporting_officer_name, reporting_officer_contact, agency_name, office_address, id_card_validity,  service_status 
        FROM eim_employee 
        WHERE service_status='Active' and record_status=1 and mobile_number = @MobileNumber";

        var result = await db.QueryFirstOrDefaultAsync<EmployeeLoginInfo>(sql, new { MobileNumber = mobileNumber });


        if (result == null)
        {
            return new EmployeeLoginInfo
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
            result.OTP_Code = "4567"; 
            result.Trx_Code = "00";
            result.Trx_Status = "Successful transaction";
        }

        return result;
    } 
    
}


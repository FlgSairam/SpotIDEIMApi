using Dapper;
using DapperAuthApi.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Data;

namespace DapperAuthApi.Repositories;

public class EmployeePerformanceRepository
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public EmployeePerformanceRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString);

    public async Task<IEnumerable<EmployeePerformance>> GetAllAsync()
    {
        using var db = Connection;
        string sql = "SELECT * FROM eim_employee_performance WHERE record_status = 1";
        return await db.QueryAsync<EmployeePerformance>(sql);
    }

    public async Task<IEnumerable<EmployeePerformance>> GetByIdAsync(long employee_Id, int qry_month)
    {
        using var db = Connection;
        string sql = "SELECT * FROM eim_employee_performance PARTITION (P" + qry_month + ") WHERE record_status = 1 AND employee_id = @Eid";
        return await db.QueryAsync<EmployeePerformance>(sql, new { Eid = employee_Id });
    }

    public async Task<EmployeePerformanceResponse> CreateAsync(EmployeePerformance entity)
    {
        using var db = Connection;
        string sql = @"INSERT INTO eim_employee_performance 
        (employee_fid, employee_id, performance_date, discom_fid, discom, division_fid, division, 
        work_location_fid, work_location, department, noof_billissued, noof_exeptionbillissued, 
        noof_transaction, amount_collected, remark, qrydate, qrymonth,  created_by) 
        VALUES 
        (@Employee_Fid, @Employee_Id, @Performance_Date, @Discom_Fid, @Discom, @Division_Fid, @Division, 
        @Work_Location_Fid, @Work_Location, @Department, @NoOf_BillIssued, @NoOf_ExceptionBillIssued, 
        @NoOf_Transaction, @Amount_Collected, @Remark, @QryDate, @QryMonth, @Created_By)";

        var affected = await db.ExecuteAsync(sql, entity);

        return new EmployeePerformanceResponse
        {
            Trx_Code = affected > 0 ? "00" : "01",
            Trx_Status = affected > 0 ? "Record inserted successfully" : "Failed to insert record"
        };
    }

    public async Task<EmployeePerformanceResponse> UpdateAsync(EmployeePerformance entity)
    {
        using var db = Connection;
        string sql = @"UPDATE eim_employee_performance SET 
            employee_fid = @Employee_Fid,
            employee_id = @Employee_Id,
            performance_date = @Performance_Date,
            discom_fid = @Discom_Fid,
            discom = @Discom,
            division_fid = @Division_Fid,
            division = @Division,
            work_location_fid = @Work_Location_Fid,
            work_location = @Work_Location,
            department = @Department,
            noof_billissued = @NoOf_BillIssued,
            noof_exeptionbillissued = @NoOf_ExceptionBillIssued,
            noof_transaction = @NoOf_Transaction,
            amount_collected = @Amount_Collected,
            remark = @Remark,
            qrydate = @QryDate,
            qrymonth = @QryMonth,
            updated_by = @Updated_By
        WHERE pid = @Pid";

        var affected = await db.ExecuteAsync(sql, entity);

        return new EmployeePerformanceResponse
        {
            Trx_Code = affected > 0 ? "00" : "01",
            Trx_Status = affected > 0 ? "Record updated successfully" : "Failed to update record"
        };
    }

    public async Task<EmployeePerformanceResponse> DeleteAsync(long pid)
    {
        using var db = Connection;
        string sql = "DELETE FROM eim_employee_performance WHERE pid = @Pid";
        var affected = await db.ExecuteAsync(sql, new { Pid = pid });

        return new EmployeePerformanceResponse
        {
            Trx_Code = affected > 0 ? "00" : "01",
            Trx_Status = affected > 0 ? "Record deleted successfully" : "Failed to delete record"
        };
    }
}

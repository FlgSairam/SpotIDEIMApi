using Dapper;
using DapperAuthApi.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Data;

namespace DapperAuthApi.Repositories;

public class EmployeeAttendanceRepository
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public EmployeeAttendanceRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new MySqlConnection(_connectionString); 
  
    public async Task<IEnumerable<EmployeeAttendance>> GetAllAsync()
    {
        using var db = Connection;
        string sql = "SELECT * FROM eim_employee_attendance WHERE record_status=1";
        return await db.QueryAsync<EmployeeAttendance>(sql);
    }

    public async Task<IEnumerable<EmployeeAttendance>> GetByIdAsync(long employee_Id, int qry_month)
    {
        using var db = Connection;
        string sql = "SELECT * FROM eim_employee_attendance partition (P"+ qry_month + ") WHERE record_status=1  and employee_Id = @Eid";
        return await db.QueryAsync<EmployeeAttendance>(sql, new { Eid = employee_Id });
    }
    public async Task<EmployeeAttendanceResponse> CreateAsync(EmployeeAttendance entity)
    {
        using var db = Connection;
        string sql = @"INSERT INTO eim_employee_attendance 
        (employee_fid, employee_id, selfieephoto_url, attendance_date, discom, work_location_fid, work_latitude, work_longitude, attendance_status, leave_type, sick_doccopy, absent_remark, qrydate, qrymonth, created_by)
        VALUES (@Employee_Fid, @Employee_Id, @SelfieePhoto_Url, @Attendance_Date, @Discom, @Work_Location_Fid, @Work_Latitude, @Work_Longitude, @Attendance_Status, @Leave_Type, @Sick_DocCopy, @Absent_Remark, @QryDate, @QryMonth, @Created_By)";
         
        var affected = await db.ExecuteAsync(sql, entity); 
         
            return new EmployeeAttendanceResponse
            {
                Trx_Code = affected > 0 ? "00" : "01",
                Trx_Status = affected > 0 ? "Record inserted successfully" : "Failed to insert record"
            };  
    }

    public async Task<EmployeeAttendanceResponse> UpdateAsync(EmployeeAttendance entity)
    {
        using var db = Connection;
        string sql = @"UPDATE eim_employee_attendance SET 
            employee_fid = @Employee_Fid,
            employee_id = @Employee_Id,
            selfieephoto_url = @SelfieePhoto_Url,
            attendance_date = @Attendance_Date,
            discom = @Discom,
            work_location_fid = @Work_Location_Fid,
            work_latitude = @Work_Latitude,
            work_longitude = @Work_Longitude,
            attendance_status = @Attendance_Status,
            leave_type = @Leave_Type,
            sick_doccopy = @Sick_DocCopy,
            absent_remark = @Absent_Remark,
            qrydate = @QryDate,
            qrymonth = @QryMonth,
            updated_by = @Updated_By
        WHERE pid = @Pid";
        var affected = await db.ExecuteAsync(sql, entity);

        return new EmployeeAttendanceResponse
        {
            Trx_Code = affected > 0 ? "00" : "01",
            Trx_Status = affected > 0 ? "Record updated successfully" : "Failed to update record"
        };
    }

    public async Task<EmployeeAttendanceResponse> DeleteAsync(long pid)
    {
        using var db = Connection;
        string sql = "DELETE FROM eim_employee_attendance WHERE pid = @Pid";
        var affected = await db.ExecuteAsync(sql, new { Pid = pid });

        return new EmployeeAttendanceResponse
        {
            Trx_Code = affected > 0 ? "00" : "01",
            Trx_Status = affected > 0 ? "Record deleted successfully" : "Failed to delete record"
        };
    }
}

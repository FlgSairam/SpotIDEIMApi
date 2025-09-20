using Dapper;
using DapperAuthApi.DBContext;
using DapperAuthApi.Interfaces;
using DapperAuthApi.Models;
using System;
using System.Data;


namespace DapperAuthApi.Repositories
{
    public class LocationLogRepository : ILocationLog
    {
        private readonly DapperDbContext _context;

        public LocationLogRepository(DapperDbContext context)
        {
            _context = context;
        }
        public async Task<int> InsertAsync(LocationLog log)
        {
            using var connection = _context.CreateConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("p_employee_fid", log.EmployeeFid, DbType.Int64);
            parameters.Add("p_mobile_number", log.MobileNumber, DbType.String);
            parameters.Add("p_work_location_fid", log.WorkLocationFid, DbType.Int64);
            parameters.Add("p_latitude", log.Latitude, DbType.Decimal);
            parameters.Add("p_longitude", log.Longitude, DbType.Decimal);
            parameters.Add("p_capturedtime", log.CapturedTime, DbType.DateTime);
            parameters.Add("p_device_id", log.DeviceId, DbType.String); 
            parameters.Add("p_battery_level", log.BatteryLevel, DbType.Int64); 
            parameters.Add("p_created_by", log.CreatedBy, DbType.Int64);
           
            var commandDefinition = new CommandDefinition("eim_location_logs_sp_insert", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 120);
            var newPid = await connection.ExecuteScalarAsync<int>(commandDefinition);
            return newPid;
        }

        public async Task<IEnumerable<LocationLogViewModel>> GetByEmployeeAsync(long employeeFid, int qryDate)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_employee_fid", employeeFid, DbType.Int64);
            parameters.Add("p_qrydate", qryDate, DbType.Int32);

            var result = await connection.QueryAsync<LocationLogViewModel>(
                "CALL eim_location_logs_sp_select_byemployee(@p_employee_fid, @p_qrydate);",
                parameters,
                commandType: CommandType.Text,  // Note: must use Text because it's a CALL
                commandTimeout: 120
            );

            return result;
        }

    }
}

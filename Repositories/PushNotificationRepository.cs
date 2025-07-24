using Dapper;
using DapperAuthApi.DBContext;
using DapperAuthApi.Interfaces;
using DapperAuthApi.Models;
using System.Data;

namespace DapperAuthApi.Repositories
{
    public class PushNotificationRepository : IPushNotification,IAttendanceRepository
    {
         private readonly DapperDbContext context;

        public PushNotificationRepository(DapperDbContext context)
        {
            this.context = context;
        }


        public async Task<SaveResult> InsertAsync(DeviceInfo entity)
        {
            using var connection = context.CreateConnection();
            DynamicParameters parameters = new();
            parameters.Add("p_employee_Fid", entity.EmployeeFid, DbType.Int64);
            parameters.Add("p_employee_id", entity.EmployeeId, DbType.Int64);
            parameters.Add("p_device_Id", entity.DeviceId, DbType.String);
            parameters.Add("p_Active", "1", DbType.Int16);

            var commandDefinition = new CommandDefinition("UpsertDeviceInfo", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 120);
            int resultCode = await connection.ExecuteAsync(commandDefinition);
            return SaveResult.FromStatus(resultCode);
        }

        public async Task<List<string>> GetById(int id)
        {
            using var connection = context.CreateConnection();

            var sql = "SELECT device_Id FROM device_id WHERE employee_id = @employee_id";

            var result = await connection.QueryAsync<string>(sql, new { employee_id = id });

            return result.ToList();
        }

        public async Task<IEnumerable<DeviceToken>> GetEmployeesWithoutAttendance(string date)
        {
            using var connection = context.CreateConnection();

            string sql = @"
                SELECT e.employee_id, e.full_name, d.device_Id 
                FROM eim_employee e
                LEFT JOIN device_id d ON d.employee_id = e.employee_id
                WHERE NOT EXISTS (
                    SELECT 1 FROM eim_employee_attendance a
                    WHERE a.employee_id = e.employee_id
                      AND a.qrydate =@date
                        )";

            var result = await connection.QueryAsync<DeviceToken>(sql, new { date });
            return result;
        }

        public async Task<object> Getdeviceids()
        {
            using var connection = context.CreateConnection();

            string sql = @"
                SELECT e.employee_id, e.full_name, d.device_Id 
                FROM eim_employee e
                LEFT JOIN device_id d ON d.employee_id = e.employee_id";

            var result = await connection.QueryAsync<object>(sql);
            return result;
        }
    }
}

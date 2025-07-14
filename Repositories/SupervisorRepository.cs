using Dapper;
using DapperAuthApi.DBContext;
using DapperAuthApi.Interfaces;
using DapperAuthApi.Models;
using System.Data;
using static Dapper.SqlMapper;

namespace DapperAuthApi.Repositories
{
    public class SupervisorRepository : ISupervisor
    {
        public readonly DapperDbContext _context;

        public SupervisorRepository(DapperDbContext context) 
        { 
            _context = context; 
        }
        public async Task<List<SvAttendanceResponse>> GetById(SvAttendance id)
        {
            using var connection = _context.CreateConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("p_qrydate", id.qrydate, DbType.Int32);
            parameters.Add("p_reporting_officer_name", id.reporting_officer_name, DbType.String);

            var commandDefinition = new CommandDefinition("Sup_Emp_AttendanceTrack", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 120);
            var response = await connection.QueryAsync<SvAttendanceResponse>(commandDefinition);
            return response.ToList();
        }

        public async Task<List<SvPerformanceResponse>> GetPerformance(SvPerformance performance)
        {
            using var connection = _context.CreateConnection();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("p_qrydate", performance.qrydate, DbType.Int32);
            parameters.Add("p_reporting_officer_name", performance.reporting_officer_name, DbType.String); 

            var commandDefinition = new CommandDefinition("Sup_Emp_PerformanceTrack", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 120);
            var response = await connection.QueryAsync<SvPerformanceResponse>(commandDefinition);
            return response.ToList();
        }
    }
}

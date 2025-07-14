using DapperAuthApi.Models;

namespace DapperAuthApi.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<DeviceToken>> GetEmployeesWithoutAttendance(string date);
    }
}

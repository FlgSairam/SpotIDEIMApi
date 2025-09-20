using DapperAuthApi.Models;

namespace DapperAuthApi.Interfaces
{
    public interface ILocationLog
    {
        Task<int> InsertAsync(LocationLog log);
        Task<IEnumerable<LocationLogViewModel>> GetByEmployeeAsync(long employeeFid, int qryDate);
    }
}

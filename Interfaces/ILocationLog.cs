using DapperAuthApi.Models;

namespace DapperAuthApi.Interfaces
{
    public interface ILocationLog
    {
        Task<int> InsertAsync(LocationLog log);
    }
}

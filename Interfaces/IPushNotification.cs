
using DapperAuthApi.Models;

namespace DapperAuthApi.Interfaces
{
    public interface IPushNotification : IRepositoryGetbyId<string, int>,IRepositoryInsert<SaveResult, DeviceInfo>
    {
        Task<object> Getdeviceids();                   
    }
   
}

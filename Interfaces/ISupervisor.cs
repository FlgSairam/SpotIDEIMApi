using DapperAuthApi.Models;

namespace DapperAuthApi.Interfaces
{
    
    public interface ISupervisor: IRepositoryGetbyId<SvAttendanceResponse, SvAttendance >
    {
        Task<List<SvPerformanceResponse>> GetPerformance(SvPerformance performance);
    }
}

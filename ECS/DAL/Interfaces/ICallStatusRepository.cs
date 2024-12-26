using ECS.Areas.EmployeeService.Models;

namespace ECS.DAL.Interfaces
{
    public interface ICallStatusRepository
    {
        Task<List<CallStatus>> GetAllCallStatus();
        Task AddCallStatus (CallStatus callStatus);
        Task UpdateCallStatus (CallStatus callStatus);
        Task DeleteCallStatus (int callStatusId);
        Task<CallStatus> GetCallStatusById (int callStatusId);
    }
}

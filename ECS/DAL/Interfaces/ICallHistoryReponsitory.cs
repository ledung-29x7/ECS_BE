using ECS.Areas.EmployeeService.Models;

namespace ECS.DAL.Interfaces
{
    public interface ICallHistoryReponsitory
    {
        Task<List<CallHistory>> GetCallHistories();
        Task<CallHistory> GetCallHistorisbyId(int id);
        Task<int> AddCallHistory (CallHistory callHistory);
        Task DeleteCallHistory (int id);
        Task UpdateCallHistory (CallHistory callHistory);

        //Task<List<CallHistory>> GetCallHistoryByEmployeeId(Guid id);

    }
}

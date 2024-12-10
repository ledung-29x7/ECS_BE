using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class CallHistoryReponsitory : ICallHistoryReponsitory
    {
        private readonly ECSDbContext _dbContext;

        public CallHistoryReponsitory(ECSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CallHistory>> GetCallHistories()
        {
            return await _dbContext.callHistory
                .FromSqlRaw("EXEC dbo.GetCallHistory")
                .ToListAsync();
        }

        public async Task<CallHistory> GetCallHistorisbyId(int id)
        {
            var callHistory = await _dbContext.callHistory
                .FromSqlRaw("EXEC dbo.GetCallHistoryById @CallId",
                    new SqlParameter("@CallId", id))
                .ToListAsync();

            return callHistory.FirstOrDefault();
          
        }

        public async Task AddCallHistory(CallHistory callHistory)
        {

            var employee_param = new SqlParameter("@EmployeeId", callHistory.EmployeeId);
            var phonenumber_param = new SqlParameter("@PhoneNumber", callHistory.PhoneNumber);
            var status_param = new SqlParameter("@Status", callHistory.Status);
            var note_param = new SqlParameter("@Notes", callHistory.Notes);
    

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC dbo.AddCallHistory @EmployeeId, @PhoneNumber, @Status, @Notes", employee_param , phonenumber_param, status_param , note_param
                );
        }

        public async Task DeleteCallHistory(int id)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC dbo.DeleteCallHistory @CallId",
                new SqlParameter("@CallId", id));
        }

        public async Task UpdateCallHistory(CallHistory callHistory)
        {
            var parameters = new[]
            {
                new SqlParameter("@CallId", callHistory.CallId),
                new SqlParameter("@EmployeeId", callHistory.EmployeeId),
                new SqlParameter("@PhoneNumber", callHistory.PhoneNumber),
                new SqlParameter("@Status", callHistory.Status),
                new SqlParameter("@Notes", callHistory.Notes)
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC dbo.UpdateCallHistory @CallId, @EmployeeId, @PhoneNumber, @Status, @Notes",
                parameters);
        }
    }
}

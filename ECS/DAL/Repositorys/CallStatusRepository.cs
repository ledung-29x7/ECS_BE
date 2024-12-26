using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class CallStatusRepository : ICallStatusRepository
    {
        private ECSDbContext _context;
        public CallStatusRepository(ECSDbContext context) 
        {
            _context = context;
        }
        public async Task AddCallStatus(CallStatus callStatus)
        {
            var name_param = new SqlParameter("@StatusName", callStatus.StatusName);
            await _context.Database.ExecuteSqlRawAsync("EXEC AddCallStatus @StatusName", name_param);
        }

        public async Task DeleteCallStatus(int callStatusId)
        {
            var id_Param = new SqlParameter("@StatusId", callStatusId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteCallStatus @StatusId", id_Param);
        }

        public async Task<List<CallStatus>> GetAllCallStatus()
        {
            return await Task.FromResult(_context.callStatuses.FromSqlRaw("EXECUTE dbo.GetAllCallStatus").ToList());
        }

        public async Task<CallStatus> GetCallStatusById(int callStatusId)
        {
            var id_param = new SqlParameter("@StatusId", callStatusId);
            var callStatuses = await _context.callStatuses
               .FromSqlRaw("EXECUTE dbo.GetCallStatusById @StatusId", id_param)
               .ToListAsync();
            return callStatuses.FirstOrDefault();
        }

        public async Task UpdateCallStatus(CallStatus callStatus)
        {
            var id_param = new SqlParameter("@StatusId", callStatus.StatusId);
            var name_param = new SqlParameter("@StatusName", callStatus.StatusName);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateCallStatus @StatusId, @StatusName", id_param, name_param);

        }
    }
}

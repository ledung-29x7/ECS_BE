using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECS.Areas.EmployeeService.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class CallHistoryRepository : ICallHistoryRepository
    {
        public readonly ECSDbContext _context;

        public CallHistoryRepository(ECSDbContext context)
        {
            _context = context;
        }

        public async Task UpdateCallHistoryAsync(int callId,int status, string notes)
        {
            var CallHistory_Param = new SqlParameter("@CallId", callId);
            var CallStatus_Param = new SqlParameter("@Status", status);
            var Notes_Param = new SqlParameter("@Notes", notes);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateCallHistoryAsync @CallId,@CallStatus, @Notes", CallHistory_Param, CallStatus_Param, Notes_Param);
        }
    }
}
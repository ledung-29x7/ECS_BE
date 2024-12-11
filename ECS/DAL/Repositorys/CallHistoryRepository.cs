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
            try
            {
                var CallHistory_Param = new SqlParameter("@CallId", callId);
                var Status_Param = new SqlParameter("@Status", status);
                var Notes_Param = new SqlParameter("@Notes", notes);
                
                Console.WriteLine($"Executing SQL: EXECUTE dbo.UpdateCallHistoryAsync @CallId={callId}, @Status={status}, @Notes={notes}");
                await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateCallHistoryAsync @CallId,@Status, @Notes", CallHistory_Param, Status_Param, Notes_Param);
                Console.WriteLine("Update successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            
        }
    }
}
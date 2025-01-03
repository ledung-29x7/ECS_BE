﻿using ECS.Areas.EmployeeService.Models;
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

        //public async Task<int> AddCallHistory(CallHistory callHistory)
        //{

        //    var employee_param = new SqlParameter("@EmployeeId", callHistory.EmployeeId);
        //    var phonenumber_param = new SqlParameter("@PhoneNumber", callHistory.PhoneNumber);
        //    var status_param = new SqlParameter("@Status", callHistory.Status);
        //    var note_param = new SqlParameter("@Notes", callHistory.Notes);

        //    // Sử dụng SqlRaw để nhận kết quả trả về
        //    var result = await _dbContext.callHistory
        //        .FromSqlRaw("EXEC dbo.AddCallHistory @EmployeeId, @PhoneNumber, @Status, @Notes",
        //            employee_param, phonenumber_param, status_param, note_param)
        //        .ToListAsync();

        //    // Lấy CallId từ kết quả trả về
        //    return result.FirstOrDefault()?.CallId ?? 0;
        //}
        public async Task<int> AddCallHistory(CallHistory callHistory)
        {
            var commandText = "EXEC dbo.AddCallHistory @EmployeeId, @PhoneNumber, @Status, @Notes";

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = commandText;
                command.CommandType = System.Data.CommandType.Text;

                // Thêm tham số
                command.Parameters.Add(new SqlParameter("@EmployeeId", callHistory.EmployeeId));
                command.Parameters.Add(new SqlParameter("@PhoneNumber", callHistory.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@Status", callHistory.Status));
                command.Parameters.Add(new SqlParameter("@Notes", callHistory.Notes));

                if (command.Connection.State != System.Data.ConnectionState.Open)
                {
                    await command.Connection.OpenAsync();
                }

                // Thực thi và lấy giá trị trả về
                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
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

        public async Task<List<CallHistory>> GetCallHistoryByEmployeeId(Guid id)
        {
            return await _dbContext.callHistory
                .FromSqlRaw("EXEC dbo.GetCallHistoryByEmployeeId @EmployeeId",
                    new SqlParameter("@EmployeeId", id))
                .ToListAsync();
        }
    }
}

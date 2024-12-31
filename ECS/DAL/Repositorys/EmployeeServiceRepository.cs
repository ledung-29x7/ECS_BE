using ECS.Areas.Admin.Models;
using System.Data;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ECS.Dtos;

namespace ECS.DAL.Repositorys
{
    public class EmployeeServiceRepository : IEmployeeServiceRepository
    {
        private readonly ECSDbContext _ecsdbContext;

        public EmployeeServiceRepository(ECSDbContext ecsdbContext)
        {
            _ecsdbContext = ecsdbContext;
        }

        public async Task<List<EmployeeService>> GetAllEmployeeAsync()
        {

            var employee = await _ecsdbContext.employeeService
          .FromSqlRaw("EXEC GetAllEmployeeServices ")
          .ToListAsync();
            return employee;
        }

        public async Task<EmployeeService> GetEmployeeByIdAsync(int id)
        {
            var param = new SqlParameter("@EmployeeServiceId", id);
            var result = await _ecsdbContext.employeeService
                .FromSqlRaw("EXEC dbo.GetEmployeeServiceById @EmployeeServiceId", param)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task AddEmployeeAsync(EmployeeService employeeService)
        {
            var employye_Param = new SqlParameter("@EmployeeId", employeeService.EmployeeId);
            var service_Param = new SqlParameter("@ProductServiceId", employeeService.ProductServiceId);
     

            await _ecsdbContext.Database.ExecuteSqlRawAsync(
                "EXEC dbo.AddEmployeeService @EmployeeId, @ProductServiceId",
                employye_Param, service_Param);

        }
   

        public async Task UpdateEmployeeAsync(EmployeeService employeeService)
        {
            var employeeService_Param = new SqlParameter("@EmployeeServiceId" , employeeService.EmployeeServiceId);
            var employye_Param = new SqlParameter("@EmployeeId", employeeService.EmployeeId);
            var service_Param = new SqlParameter("@ProductServiceId", employeeService.ProductServiceId);


            await _ecsdbContext.Database.ExecuteSqlRawAsync(
                "EXEC dbo.UpdateEmployeeService @EmployeeServiceId , @EmployeeId, @ProductServiceId",
               employeeService_Param, employye_Param, service_Param);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var param = new SqlParameter("@EmployeeServiceId", id);
            await _ecsdbContext.Database.ExecuteSqlRawAsync("EXEC dbo.DeleteEmployeeService @EmployeeServiceId", param);
        }
    }
}

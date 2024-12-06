using ECS.Areas.Admin.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ECSDbContext _context;

        public DepartmentRepository(ECSDbContext context) 
        {
            _context = context;
        }
        public async Task AddDepartments(Departments departments)
        {

            var Name_param = new SqlParameter("@DepartmentName", departments.DepartmentName);
            // var Manager_param = new SqlParameter("@DepartmentName", departments.ManagerId);
            var Manager_param = departments.ManagerId == null
             ? new SqlParameter("@ManagerId", DBNull.Value)
             : new SqlParameter("@ManagerId", departments.ManagerId);
            await _context.Database.ExecuteSqlRawAsync("EXEC AddDepartments @DepartmentName, @ManagerId", Name_param, Manager_param);
        }

        public async Task DeleteDepartments(int departmentsId)
        {
            var id_Param = new SqlParameter("@DepartmentId", departmentsId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteDepartments @DepartmentId", id_Param);
        }

        public async Task<List<Departments>> GetAllDepartments()
        {
            return await Task.FromResult(_context.departments.FromSqlRaw("EXECUTE dbo.GetAllDepartments").ToList());
        }

        public async Task<Departments> GetDepartmentsById(int departmentsId)
        {
            var id_Param = new SqlParameter("@DepartmentId", departmentsId);
            var departments = await _context.departments
                            .FromSqlRaw("EXECUTE dbo.GetDepartmentsById @DepartmentId", id_Param)
                            .ToListAsync();
            return departments.FirstOrDefault();
        }

        public async Task SetManagerForDepartment(int departmentsId, Guid managerId)
        {
            var departmentsId_param = new SqlParameter("@DepartmentsId", departmentsId);
            var manageId_param = new SqlParameter("@ManagerId", managerId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.SetManagerForDepartment @DepartmentsId, @ManagerId", departmentsId_param, manageId_param);
        }

        public async Task UpdateDepartments(Departments departments)
        {
            var id_Param = new SqlParameter("@DepartmentId", departments.DepartmentID);
            var Name_param = new SqlParameter("@DepartmentName", departments.DepartmentName);
            var Manager_param = new SqlParameter("@ManagerId", departments.ManagerId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateDepartments @DepartmentId, @DepartmentName, @ManagerId", id_Param, Name_param, Manager_param);
        }

        
    }
}

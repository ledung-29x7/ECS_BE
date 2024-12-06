using ECS.Areas.Authen.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ECSDbContext _context;

        public EmployeeRepository(ECSDbContext context) 
        {
            _context = context;
        }

        public async Task DeleteEmployee(Guid employeeid)
        {
            var id_param = new SqlParameter("@EmployeeId", employeeid);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteEmployee @RoleId", id_param);
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            var email_Param = new SqlParameter("@Email", email);
            var users = await _context.employees.FromSqlRaw("EXEC GetEmployeeByEmail @Email", email_Param).ToListAsync();
            return users.FirstOrDefault();
        }

        public async Task RegisterEmployee(Employee employee)
        {
            var FirstName_param = new SqlParameter("@FirstName", employee.FirstName);
            var LastName_param = new SqlParameter("@LastName", employee.LastName);
            var email_param = new SqlParameter("@Email", employee.Email);
            var Phone_number_param = new SqlParameter("@PhoneNumber", employee.PhoneNumber);
            var Password_param = new SqlParameter("@Password", employee.Password);
            await _context.Database.ExecuteSqlRawAsync("EXEC RegisterEmployee @FirstName, @LastName, @Email, @PhoneNumber, @Password", FirstName_param, LastName_param, email_param, Phone_number_param, Password_param);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var id_param = new SqlParameter("@EmployeeId", employee.EmployeeId);
            var FirstName_param = new SqlParameter("@FirstName", employee.FirstName);
            var LastName_param = new SqlParameter("@LastName", employee.LastName);
            var Email_param = new SqlParameter("@Email", employee.Email);
            var PhoneNumber_param = new SqlParameter("@PhoneNumber", employee.PhoneNumber);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateEmployee @EmployeeId, @FirstName, @LastName, @Email, @PhoneNumber", id_param, FirstName_param, LastName_param, Email_param, PhoneNumber_param);
        }

        public async Task UpdateEmployeeRole(Guid EmployeeId, int RoleId)
        {
            var id_param = new SqlParameter("@EmployeeId", EmployeeId);
            var RoleId_param = new SqlParameter("@RoleId", RoleId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateEmployeeRole @EmployeeId, @RoleId ", id_param, RoleId_param );
        }

        public async Task UpdateDepartmentForEmployee(Guid EmployeeId, int departmentsId)
        {
            var EmployeeId_Param = new SqlParameter("@EmployeeId", EmployeeId);
            var departmentsId_Param = new SqlParameter("@DepartmentsId", departmentsId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateDepartmentForEmployee @EmployeeId, @DepartmentsId", EmployeeId_Param, departmentsId_Param);
        }
    }
}

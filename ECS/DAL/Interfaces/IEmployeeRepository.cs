using ECS.Areas.Authen.Models;
using ECS.Dtos;

namespace ECS.DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        Task RegisterEmployee(Employee employee);
        Task<Employee> GetEmployeeByEmail(string email);
        Task DeleteEmployee(Guid employeeid);
        Task UpdateEmployee(Employee employee);
        Task UpdateEmployeeRole(Guid EmployeeId, int RoleId);
        Task UpdateDepartmentForEmployee(Guid EmployeeId, int departmentsId);
        Task DeleteEmployeeAndUnsetManager(Guid EmployeeId);
        Task<List<EmployeeWorkListDto>> GetEmployeeWorkListByIdAsync(Guid employeeId);

        Task<List<Employee>> GetAllEmployee();
    }
}

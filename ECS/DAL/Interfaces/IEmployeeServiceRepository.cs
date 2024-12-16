using ECS.Areas.Admin.Models;
using ECS.Dtos;

namespace ECS.DAL.Interfaces
{
    public interface IEmployeeServiceRepository
    {
        Task<List<EmployeeService>> GetAllEmployeeAsync();
        Task<EmployeeService> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(EmployeeService employeeService);
        Task UpdateEmployeeAsync(EmployeeService employeeService);
        Task DeleteEmployeeAsync(int id);
    }
}

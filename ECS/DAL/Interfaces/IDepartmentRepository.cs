using ECS.Areas.Admin.Models;

namespace ECS.DAL.Interfaces
{
    public interface IDepartmentRepository
    {
        Task AddDepartments(Departments departments);
        Task<List<Departments>> GetAllDepartments();
        Task UpdateDepartments (Departments departments);
        Task DeleteDepartments (int departmentsId);
        Task<Departments> GetDepartmentsById(int departmentsId);

        Task SetManagerForDepartment(int departmentsId, Guid managerId);
        
    }
}

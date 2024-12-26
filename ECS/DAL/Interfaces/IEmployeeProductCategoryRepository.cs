using ECS.Areas.Authen.Models;
using ECS.Areas.Client.Models;

namespace ECS.DAL.Interfaces
{
    public interface IEmployeeProductCategoryRepository
    {
        Task<List<EmployeeProductCategory>> GetAllEmployeeProductCategories();
        Task AddEmployeeProductCategory(EmployeeProductCategory employeeProductCategory);
        Task DeleteEmployeeProductCategory(int employeeProductCategoryId);

        Task<EmployeeProductCategory> GetEmployeeProductCategoryById (int employeeProductCategoryId);
        Task<List<Employee>> GetEmployeeForProductCategoryByCategoryId(int categoryId);
        Task<List<ProductCategory>> GetProductCategoryByEmployeeId(Guid employeeId);
    }
}

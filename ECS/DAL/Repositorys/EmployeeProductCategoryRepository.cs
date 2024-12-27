using ECS.Areas.Authen.Models;
using ECS.Areas.Client.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class EmployeeProductCategoryRepository : IEmployeeProductCategoryRepository
    {
        private readonly ECSDbContext _context;
        public EmployeeProductCategoryRepository(ECSDbContext context)
        {
            _context = context;
        }
        public async Task AddEmployeeProductCategory(EmployeeProductCategory employeeProductCategory)
        {
            var employeeId_param = new SqlParameter("@EmployeeId", employeeProductCategory.EmployeeId);
            var categoryId_param = new SqlParameter("@CategoryId", employeeProductCategory.CategoryId);
            await _context.Database.ExecuteSqlRawAsync("EXEC AddEmployeeProductCategory @EmployeeId, @CategoryId", employeeId_param, categoryId_param);
        }

        public async Task DeleteEmployeeProductCategory(int employeeProductCategoryId)
        {
            var id_param = new SqlParameter("@EmployeeProductCategoryId", employeeProductCategoryId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteEmployeeProductCategory @EmployeeProductCategoryId", id_param);
        }

        public async Task<List<EmployeeProductCategory>> GetAllEmployeeProductCategories()
        {
            return await Task.FromResult(_context.employeeProductCategories.FromSqlRaw("EXECUTE dbo.GetAllEmployeeProductCategories").ToList());
        }

        public async Task<List<Employee>> GetEmployeeForProductCategoryByCategoryId(int categoryId)
        {
            var categoryId_Param = new SqlParameter("@CategoryId", categoryId);
            var employees = await _context.employees
              .FromSqlRaw("EXECUTE dbo.GetEmployeeForProductCategoryByCategoryId @CategoryId", categoryId_Param)
              .ToListAsync();
            return employees;
        }

        public async Task<List<ProductCategory>> GetProductCategoryByEmployeeId(Guid employeeId)
        {
            var employeeId_param = new SqlParameter("@EmployeeId", employeeId);
            var categorys = await _context.ProductCategory
                .FromSqlRaw("EXECUTE dbo.GetProductCategoryByEmployeeId @EmployeeId", employeeId_param)
                .ToListAsync();
            return categorys;
        }

        public async Task<EmployeeProductCategory> GetEmployeeProductCategoryById(int employeeProductCategoryId)
        {
            var employeeProductCategoryId_param = new SqlParameter("@EmployeeProductCategoryId", employeeProductCategoryId);
            var employeeProductCategories = await _context.employeeProductCategories
               .FromSqlRaw("EXECUTE dbo.GetEmployeeProductCategoryById @EmployeeProductCategoryId", employeeProductCategoryId_param)
               .ToListAsync();
            return employeeProductCategories.FirstOrDefault();
        }

        public async Task UpdateEmployeeProductCategoryByEmployeeId(EmployeeProductCategory employeeProductCategory)
        {
            var employeeId_param = new SqlParameter("@EmployeeId", employeeProductCategory.EmployeeId);
            var CategoryId_param = new SqlParameter("@Category", employeeProductCategory.CategoryId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateEmployeeProductCategory @EmployeeId, @Category", employeeId_param, CategoryId_param);
        }
    }
}

using ECS.Areas.Authen.Models;
using ECS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECS.DAL.Repositorys
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ECSDbContext _context;

        public RoleRepository(ECSDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task AddRole(Role role)
        {
            var name_param = new SqlParameter("@RoleName", role.RoleName);
            var salary_param = new SqlParameter("@BaseSalary", role.BaseSalary);
            await _context.Database.ExecuteSqlRawAsync("EXEC AddRole @RoleName, @BaseSalary", name_param, salary_param);

        }

        public async Task DeleteRole(int roleId)
        {
            var id_Param = new SqlParameter("@RoleId", roleId);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.DeleteRole @RoleId", id_Param);
        }

        public async Task<List<Role>> GetAllRole()
        {
            return await Task.FromResult(_context.roles.FromSqlRaw("EXECUTE dbo.GetAllRole").ToList());
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            var id_param = new SqlParameter("@RoleId", roleId);
            var roles = await _context.roles
               .FromSqlRaw("EXECUTE dbo.GetRoleById @RoleId", id_param)
               .ToListAsync();
            return roles.FirstOrDefault();
        }

        public async Task UpdateRole(Role role)
        {
            var id_param = new SqlParameter("@RoleId", role.RoleId);
            var name_param = new SqlParameter("@RoleName", role.RoleName);
            var salary_param = new SqlParameter("@BaseSalary", role.BaseSalary);
            await _context.Database.ExecuteSqlRawAsync("EXECUTE dbo.UpdateRole @RoleId, @RoleName, @BaseSalary", id_param, name_param, salary_param );

        }
    }
}
